using DeviceContext;
using MacSave.Funcs;
using DeviceModel;
using mac_register.Models.FullDeviceCreate;
using MacSave.Models.SinalHistory;
using Microsoft.Extensions.Logging;
using Model.ProblemTreatWrapper;
using Models.UsedAtWrapper.UsedAtWrapper;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MacSave.Funcs.Database
{
	public class DatabaseTasks
	{//this class contains database options such save, create, delete and update items 
	 //it was done with goal to reduce amount of code at controller
		private readonly DeviceDb _db;
		private readonly ILogger<DatabaseTasks> _logger;
		private readonly RegexService _regexService;


		public DatabaseTasks(
			DeviceDb db,
			ILogger<DatabaseTasks> logger,
			RegexService regexService
			)
		{
			_logger = logger;
			_db = db;
			_regexService = regexService;
		}


		public DeviceCreate CreateDevice(FullDeviceCreate deviceDity)
		{//must save at database at other component
			return new DeviceCreate
			{
				DeviceId = Guid.NewGuid().ToString(),
				Mac = _regexService.SanitizeInput(deviceDity.Mac),
				Model = _regexService.SanitizeInput(deviceDity.Category_Id_Device),
				RemoteAcess = deviceDity.RemoteAcess,
				DeviceCategoryId = deviceDity.Category_Id_Device
			};
		}



		//create DeviceCreate itens before sabe it
		public async Task CreateRelatedEntities(FullDeviceCreate deviceDity, DeviceCreate macDevice)

		{
			var problem = new ProblemTreatWrapper
			{
				ProblemName = _regexService.SanitizeInput(deviceDity.ProblemName),
				ProblemDescription = _regexService.SanitizeInput(deviceDity.ProblemDescription),
				DeviceId = macDevice.DeviceId,
			};
			macDevice.AddProblems(problem);
			_logger.LogInformation($"\n\n\n-----Save  Problem-----\n\n\n");

			var userAtWrapper = new UsedAtWrapper
			{
				Name = _regexService.SanitizeInput(deviceDity.UserName),
				DeviceId = _regexService.SanitizeInput(macDevice.DeviceId),
			};
			macDevice.AddClients(userAtWrapper);
			_logger.LogInformation($"\n\n\n-----Save  Clients-----\n\n\n");


			var signalHistory = new SinalHistory
			{
				SinalRX = _regexService.SanitizeInput(deviceDity.SinalRX),
				SinalTX = _regexService.SanitizeInput(deviceDity.SinalTX),
				DeviceId = macDevice.DeviceId
			};
			macDevice.AddSinal(signalHistory);
			_logger.LogInformation($"\n\n\n-----Save  History-----\n\n\n");

			await _db.SaveChangesAsync();
			_logger.LogInformation($"\n\n\n-----Save  item at databse-----\n\n\n");
		}

	}
}