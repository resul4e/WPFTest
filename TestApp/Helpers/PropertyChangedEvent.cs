using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TestApp.Annotations;

namespace TestApp
{
	class PropertyChangedEvent : INotifyPropertyChanged
	{
		[NotifyPropertyChangedInvocator]
		protected virtual void RaisePropertyChanged([CallerMemberName] string _propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(_propertyName));
		}

		public event PropertyChangedEventHandler PropertyChanged;
	}
}
