using PropertyWriter.Annotation;
using PropertyWriter.Models.Info;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PropertyWriter.Models.Properties.Common
{
	using MasterRepository = Dictionary<string, ReferencableMasterInfo>;

	public class MasterLoader
	{
		private MasterRepository masters = new MasterRepository();
		private Dictionary<Type, Type[]> subtypes = new Dictionary<Type, Type[]>();
		private bool isLoaded = false;
		private PropertyFactory factory;

		public MasterRepository Masters
		{
			get
			{
				if (!isLoaded)
				{
					throw new InvalidOperationException("マスターを参照する前にロードを行ってください。");
				}
				return masters;
			}
		}
		public Dictionary<Type, Type[]> Subtypes
		{
			get
			{
				if (!isLoaded)
				{
					throw new InvalidOperationException("派生クラスを問い合わせる前にロードを行ってください。");
				}
				return subtypes;
			}
		}


		public MasterLoader(PropertyFactory factory)
		{
			this.factory = factory;
		}

		public PropertyRoot LoadStructure(Assembly assembly, Type projectType, Project[] dependencies)
		{
			if (!projectType.IsPublic && !projectType.IsNestedPublic)
			{
				throw new ArgumentException($"プロジェクト型 {projectType.FullName} がパブリックではありませんでした。", nameof(projectType));
			}

			LoadSubtypes(assembly);

			// マスターの情報を取得
			var masterMembers = projectType.GetMembers();
			var properties = GetMastersProperties(masterMembers);
			var masterinfo = from p in properties
							 where p.info.PropertyType.IsArray
							 select GetMasterinfo(p);
			masters = GetMasters(masterinfo);

			// 依存関係先のマスター・グローバル情報を取得
			var dependencyMasters = from p in dependencies
									from q in p.Factory.Masters
									select (key: p.Root.Value.Type.Name, value: q.Value);
			dependencyMasters.ForEach(x => masters[x.key] = x.value);

			isLoaded = true;

			// グローバルの情報を取得
			var globals = from p in properties
						  where !p.info.PropertyType.IsArray
						  select GetMasterinfo(p);

			var models = globals.Concat(masterinfo).ToArray();
			return new PropertyRoot(projectType, models);
		}


		private void LoadSubtypes(Assembly assembly)
		{
			var types = assembly.GetTypes();
			var subtypings = types.Where(Helpers.IsAnnotatedType<PwSubtypingAttribute>).ToArray();
			var subtypes = types.Where(Helpers.IsAnnotatedType<PwSubtypeAttribute>).ToArray();
			this.subtypes = subtypings.ToDictionary(x => x,
				x => subtypes.Where(y => x.IsAssignableFrom(y)).ToArray());
		}

		private MasterInfo GetMasterinfo((PropertyInfo info, PwMasterAttribute attr) property)
		{
			var key = property.attr.Key;
			var type = property.info.PropertyType;
			var title = property.attr.Name ?? property.info.Name;
			return new MasterInfo(key, property.info, factory.Create(type, title));
		}

		private MasterRepository GetMasters(IEnumerable<MasterInfo> masterinfo)
		{
			var dictionary = new MasterRepository();
			foreach (var info in masterinfo)
			{
				if (info.Master is ComplicateCollectionProperty prop)
				{
					dictionary[info.Key] = new ReferencableMasterInfo()
					{
						Collection = prop.Collection.ToReadOnlyReactiveCollection(x => x.Value.Value),
						Type = info.Property.PropertyType.GetElementType(),
					};
				}
			}
			return dictionary;
		}

		private IEnumerable<(PropertyInfo info, PwMasterAttribute attr)> GetMastersProperties(
			MemberInfo[] members)
		{
			foreach (var member in members)
			{
				var attr = member.GetCustomAttribute<PwMasterAttribute>();
				if (attr == null)
				{
					continue;
				}

				if (member.MemberType == MemberTypes.Property)
				{
					yield return ((PropertyInfo)member, attr);
				}
			}
		}
	}
}