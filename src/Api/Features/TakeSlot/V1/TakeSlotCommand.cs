namespace doctor_slot.Features.GetWeeklyAvailability.V1
{
    public class TakeSlotCommand : IRequest<TakeSlotResponse>
    {
        public TakeSlotCommand(TakeSlotModel model)
        {
            Model = model;
        }

        public TakeSlotModel Model { get; set; }
    }
}
