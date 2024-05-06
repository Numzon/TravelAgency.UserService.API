using TravelAgency.SharedLibrary.Enums;
using TravelAgency.SharedLibrary.RabbitMQ;
using TravelAgency.UserService.Infrastructure.EventStrategies;

namespace TravelAgency.UserService.Infrastructure.Configs;
public static class EventReceiverConfig
{
    public static TypeEventStrategyConfig GetGlobalSettingsConfiguration()
    {
        var config = TypeEventStrategyConfig.GlobalSetting;

        config.NewConfig<CreateEmployeeEventStrategy>(EventTypes.EmployeeCreated);
        config.NewConfig<CreateManagerEventStrategy>(EventTypes.ManagerCreated);

        return config;
    }
}
