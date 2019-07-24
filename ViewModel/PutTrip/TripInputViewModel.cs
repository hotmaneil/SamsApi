using Newtonsoft.Json;
using System;

namespace ViewModel.PutTrip
{
	/// <summary>
	/// 前端新增放趟資料用
	/// </summary>
	public class TripInputViewModel
	{
		public int Id { set; get; }

		/// <summary>
		/// 日期
		/// </summary>
		[JsonProperty("date")]
		public DateTime date { get; set; }

		/// <summary>
		/// 開放群組權限
		/// </summary>
		public int AuthotrizeType { set; get; }

		/// <summary>
		/// 凌晨12:00-早上1點的放趟數量[x,x,x,x]
		/// </summary>
		public string H0001 { set; get; }

		/// <summary>
		/// 早上1點-早上2點的放趟數量[x,x,x,x]
		/// </summary>
		public string H0102 { set; get; }

		/// <summary>
		/// 早上2點-早上3點的放趟數量[x,x,x,x]
		/// </summary>
		public string H0203 { set; get; }

		/// <summary>
		/// 早上3點-早上4點的放趟數量[x,x,x,x]
		/// </summary>
		public string H0304 { set; get; }

		/// <summary>
		/// 早上4點-早上5點的放趟數量[x,x,x,x]
		/// </summary>
		public string H0405 { set; get; }

		/// <summary>
		/// 早上5點-早上6點的放趟數量[x,x,x,x]
		/// </summary>
		public string H0506 { set; get; }

		/// <summary>
		/// 早上6點-早上7點的放趟數量[x,x,x,x]
		/// </summary>
		public string H0607 { set; get; }

		/// <summary>
		/// 早上7點-早上8點的放趟數量[x,x,x,x]
		/// </summary>
		public string H0708 { set; get; }

		/// <summary>
		/// 早上8點-早上9點的放趟數量[x,x,x,x]
		/// </summary>
		public string H0809 { set; get; }

		/// <summary>
		/// 早上9點-早上10點的放趟數量[x,x,x,x]
		/// </summary>
		public string H0910 { set; get; }

		/// <summary>
		/// 早上10點-早上11點的放趟數量[x,x,x,x]
		/// </summary>
		public string H1011 { set; get; }

		/// <summary>
		/// 早上11點-中午12點的放趟數量[x,x,x,x]
		/// </summary>
		public string H1112 { set; get; }

		/// <summary>
		/// 中午12點-下午1點的放趟數量[x,x,x,x]
		/// </summary>
		public string H1213 { set; get; }

		/// <summary>
		/// 下午1點-下午2點的放趟數量[x,x,x,x]
		/// </summary>
		public string H1314 { set; get; }

		/// <summary>
		/// 下午2點-下午3點的放趟數量[x,x,x,x]
		/// </summary>
		public string H1415 { set; get; }

		/// <summary>
		/// 下午3點-下午4點的放趟數量[x,x,x,x]
		/// </summary>
		public string H1516 { set; get; }

		/// <summary>
		/// 下午4點-下午5的放趟數量[x,x,x,x]
		/// </summary>
		public string H1617 { set; get; }

		/// <summary>
		/// 下午5點-晚上6點的放趟數量[x,x,x,x]
		/// </summary>
		public string H1718 { set; get; }

		/// <summary>
		/// 晚上6點-晚上7點的放趟數量[x,x,x,x]
		/// </summary>
		public string H1819 { set; get; }

		/// <summary>
		/// 晚上7點-晚上8點的放趟數量[x,x,x,x]
		/// </summary>
		public string H1920 { set; get; }

		/// <summary>
		/// 晚上8點-晚上9點的放趟數量[x,x,x,x]
		/// </summary>
		public string H2021 { set; get; }

		/// <summary>
		/// 晚上9點-晚上10點的放趟數量[x,x,x,x]
		/// </summary>
		public string H2122 { set; get; }

		/// <summary>
		/// 晚上10點-晚上11點的放趟數量[x,x,x,x]
		/// </summary>
		public string H2223 { set; get; }

		/// <summary>
		/// 晚上11點-晚上12點的放趟數量[x,x,x,x]
		/// </summary>
		public string H2300 { set; get; }
	}
}
