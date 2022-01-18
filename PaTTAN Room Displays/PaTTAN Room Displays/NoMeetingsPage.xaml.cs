namespace PaTTAN_Room_Displays
{
	public partial class NoMeetingsPage : ContentPage
	{
		public String Message { get; set; }

		public NoMeetingsPage()
		{
			// order of this message is importnat must be before comp init.
			BindingContext = this;
			Message = "There are no meetings scheduled today";
			InitializeComponent();			
		}
	}
}