using System.Threading.Tasks;
using ViewModel.PutTrip;
using ViewModel.VerifyViewModel;

namespace Services.Interface
{
	public interface IPutTripService
	{
		/// <summary>
		/// 新增或更新 放趟
		/// </summary>
		/// <param name="model"></param>
		/// <param name="UserId"></param>
		/// <returns></returns>
		Task<VerityResult> CreateOrUpdatePutTrip(TripInputViewModel model, string UserId);
	}
}
