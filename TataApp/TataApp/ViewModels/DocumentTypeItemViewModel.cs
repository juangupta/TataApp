using System.ComponentModel;
using TataApp.Models;

namespace TataApp.ViewModels
{
    public class DocumentTypeItemViewModel : DocumentType, INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        public int documentTypeId;
        #endregion
        #region Properties
        public int DocumentTypeId
        {
            set
            {
                if (documentTypeId != value)
                {
                    documentTypeId = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DocumentTypeId"));
                }
            }
            get
            {
                return documentTypeId;
            }
        } 
        #endregion
    }
}
