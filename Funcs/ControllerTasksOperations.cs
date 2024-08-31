
using DeviceContext;
using DeviceModel;
using mac_register.Models.FullDeviceCreate;
using MacSave.Models.SinalHistory;
using Model.ProblemTreatWrapper;
using Models.UsedAtWrapper.UsedAtWrapper;

namespace MacSave.Funcs.Database
{
	public class DatabaseTasks
	{//this class contains database options such save, create, delete and update items 
	 //it was done with goal to reduce amount of code at controller
		private readonly DeviceDb _db;
		private readonly ILogger<DatabaseTasks> _logger;
		private readonly RegexService _regexService;
		private bool _LogStatus = true;


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
		 //handle behavior if some error ocurr

			var checkMacAlreadyExists = _db.Devices.Where(d => d.Mac == deviceDity.Mac).Any();
			if (checkMacAlreadyExists)
			{
				if (_LogStatus)
				{
					_logger.LogInformation("\n\n\nAttempted to create a device with an existing MAC: {Mac}", deviceDity.Mac);
				}

				throw new InvalidOperationException("MAC address already exists in the database.");
			}

			return new DeviceCreate
			{
				DeviceId = Guid.NewGuid().ToString(),
				Mac = _regexService.SanitizeInput(deviceDity.Mac).ToUpper(),//uppercase mac as a standart 
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

			if (_LogStatus)
			{
			_logger.LogInformation($"\n\n\n-----Save  Problem-----\n\n\n");
			}

			var userAtWrapper = new UsedAtWrapper
			{
				Name = _regexService.SanitizeInput(deviceDity.UserName),
				DeviceId = _regexService.SanitizeInput(macDevice.DeviceId),
			};
			macDevice.AddClients(userAtWrapper);

			if (_LogStatus)
			{
			_logger.LogInformation($"\n\n\n-----Save  Clients-----\n\n\n");
			}


			var signalHistory = new SinalHistory
			{
				SinalRX = _regexService.SanitizeInput(deviceDity.SinalRX),
				SinalTX = _regexService.SanitizeInput(deviceDity.SinalTX),
				DeviceId = macDevice.DeviceId
			};
			macDevice.AddSinal(signalHistory);

			if (_LogStatus)
			{
			_logger.LogInformation($"\n\n\n-----Save  History-----\n\n\n");
			}

			await _db.SaveChangesAsync();

			if (_LogStatus)
			{
			_logger.LogInformation($"\n\n\n-----Save  item at databse-----\n\n\n");
			}
		}

	}
}