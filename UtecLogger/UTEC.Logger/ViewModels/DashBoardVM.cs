/*
 * Created by SharpDevelop.
 * User: Walter
 * Date: 05/28/2011
 * Time: 10:54
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ComponentModel;
using System.Windows.Input;

using MVVM;

namespace UTEC.Logger.ViewModels
{
	/// <summary>
	/// Description of DashBoardVM.
	/// </summary>
	public class DashBoardVM : ViewModelBase
	{
		public DashBoardVM()
		{
			this.PropertyChanged += DashBoardVM_PropertyChanged;
		}

		void DashBoardVM_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if(e.PropertyName == "UtecDataArray")
			{
				UpdateDashBoardDisplay();
			}
		}
		
		void UpdateDashBoardDisplay()
		{
			OnPropertyChanged("RPM");
			OnPropertyChanged("Boost");
			OnPropertyChanged("MAFV");
			OnPropertyChanged("TPS");
			OnPropertyChanged("Load");
			OnPropertyChanged("Knock");
			OnPropertyChanged("ECUIGN");
			OnPropertyChanged("IDC");
			OnPropertyChanged("IGN");
			OnPropertyChanged("MAPVE");
			OnPropertyChanged("MODMAFV");
			OnPropertyChanged("AFR");
		}
		
		
		public string UtecData
		{
			get { return this.Get<string>("UtecData"); }
			set { this.Set<string>("UtecData", value); }
		}
				
		private string[] _utecDataArray = new string[14];
		public string[] UtecDataArray
		{
			get{return _utecDataArray;}
			set
			{
				_utecDataArray = value;
				OnPropertyChanged("UtecDataArray");
			}
		}

		private string GetData(int index)
		{
			if(index < UtecDataArray.Length)
			{
				return UtecDataArray[index];
			}
			else
			{
				return string.Empty;
			}
		}
		
		public string RPM
		{
			get{return GetData(0);}
		}
		
		public string Boost
		{
			get{return GetData(1);}
		}
		
		public string MAFV
		{
			get{return GetData(2);}
		}
		
		public string TPS
		{
			get{return GetData(3);}
		}
		
		public string Load
		{
			get{return GetData(4);}
		}
		
		public string Knock
		{
			get{return GetData(5);}
		}
		
		public string ECUIGN
		{
			get{return GetData(7);}
		}
		
		public string IDC
		{
			get{return GetData(8);}
		}
		
		public string IGN
		{
			get{return GetData(9);}
		}
		
		public string MAPVE
		{
			get{return GetData(10);}
		}
		
		public string MODMAFV
		{
			get{return GetData(12);}
		}
		
		public string AFR
		{
			get{return GetData(13);}
		}
		
	}
}
