using System;

namespace Utility.Extensions
{
	public static class DbSetExt
	{
		/// <summary>
		/// 複製來源至目的之物件欄位
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="sourceObject"></param>
		/// <param name="destObject"></param>
		public static void CopyObject<T>(object sourceObject, ref T destObject)
		{
			//	If either the source, or destination is null, return
			if (sourceObject == null || destObject == null)
				return;

			//	Get the type of each object
			Type sourceType = sourceObject.GetType();
			Type targetType = destObject.GetType();

			//	Loop through the source properties
			foreach (System.Reflection.PropertyInfo p in sourceType.GetProperties())
			{
				//	Get the matching property in the destination object
				System.Reflection.PropertyInfo targetObj = targetType.GetProperty(p.Name);
				//	If there is none, skip
				if (targetObj == null)
					continue;

				//	Set the value in the destination
				if (targetObj.CanWrite)
					targetObj.SetValue(destObject, p.GetValue(sourceObject, null), null);
			}
		}
	}
}
