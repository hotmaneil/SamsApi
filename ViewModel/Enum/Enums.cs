using System.ComponentModel;

namespace ViewModel.Enum
{
	public enum ApplicationTypes
	{
		JavaScript = 0,
		NativeConfidential = 1
	}

	/// <summary>
	/// (Enum)客服相關參數, dbo.TypeCode.[TypeId] = CODE?(?帶入項目設定的數字)
	/// </summary>
	public enum ddlOpt : int
	{
		/// <summary>
		/// 經濟狀況, dbo.TypeCode.[TypeId] = CODE1
		/// </summary>
		[Description("經濟狀況")]
		經濟狀況 = 1,

		/// <summary>
		/// 郵遞區號, 
		/// </summary>
		郵遞區號 = 2,

		/// <summary>
		/// 手冊(診斷證明), dbo.TypeCode.[TypeId] = code3
		/// </summary>
		手冊 = 3,

		/// <summary>
		/// 身障等級, 身障等級 0無,特A,A,B,C(外縣市),D(在途),輕度,中度,重度,極重度
		/// </summary>
		身障等級 = 4,

		/// <summary>
		/// 身障類別: 0無,視障,聽障,肢障,多重障,平衡障,聲障,語障,聲語障,智障,器障,重器障,顏殘障,植物人,失智症,癡呆症,痴障,自閉症,精神症,頑性,罕見,其它
		/// </summary>
		身障類別 = 5,

		/// <summary>
		/// 診斷類別
		/// </summary>
		診斷類別 = 6,

		/// <summary>
		/// 可預約幾天後趟次
		/// </summary>
		預約幾天 = 7,

		/// <summary>
		/// 輪椅類別
		/// </summary>
		輪椅類別 = 8,

		/// <summary>
		/// 長期失能程度
		/// </summary>
		長期失能程度 = 9,

		/// <summary>
		/// 關係
		/// </summary>
		關係 = 10,

		/// <summary>
		/// 旅途目的
		/// </summary>
		旅途目的 = 11,

		/// <summary>
		/// 性別
		/// </summary>
		性別 = 12,

		/// <summary>
		/// 是與否選項
		/// </summary>
		是否 = 99
	}

	/// <summary>
	/// 趟次狀態
	/// </summary>
	/// <remarks> 0預約 ，1後補 </remarks>
	public enum TripStatus
	{
		/// <summary>
		/// 0預約 Reserve
		/// </summary>
		預約 = 0,

		/// <summary>
		/// 1後補 Backup
		/// </summary>
		後補 = 1,

		/// <summary>
		/// 2臨時
		/// </summary>
		臨時 = 2,

		/// <summary>
		/// 3取消
		/// </summary>
		取消 = 3,

		/// <summary>
		/// 4爽約
		/// </summary>
		爽約 = 4
	}

	/// <summary>
	/// 預約處理狀態
	/// </summary>
	public enum BookingStatus : byte
	{
		/// <summary>
		/// 新建立
		/// </summary>
		New = 0,

		/// <summary>
		/// 受理
		/// </summary>
		Accept = 1,

		/// <summary>
		/// 轉訂單
		/// </summary>
		ToOrder = 2,

		/// <summary>
		/// 取消
		/// </summary>
		Cancel = 3,

		/// <summary>
		/// 失敗
		/// </summary>
		Fail = 4,

		/// <summary>
		/// 完成
		/// </summary>
		Complete = 5,

		/// <summary>
		/// 改派
		/// </summary>
		Reassign = 6
	}
}
