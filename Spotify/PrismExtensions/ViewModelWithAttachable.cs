namespace Spotify.PrismExtensions
{
  using System;
  using System.ComponentModel;
  using System.Reflection;

  using Microsoft.Practices.Prism.Mvvm;

  class ViewModelWithAttachable<T> : ViewModel
  {
    #region Fields

    private T propAttachedDataModel;
    private bool propIsReadingDataModel;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Creates a new Instance.
    /// </summary>
    public ViewModelWithAttachable()
    {
      this.PropertyChanged += this.ViewModelWithAttachable_PropertyChanged;
    }

    #endregion Constructors

    #region Events

    public event EventHandler<EventArgs<T>> ReadingDataModel;
    public event EventHandler<EventArgs> NullingDataModel;

    #endregion Events

    #region Properties

    /// <summary>
    /// The attached data model.
    /// This should be a POCO class which will be automatically updated if a property linked to it changes.
    /// To update the ViewModel attach the model again.
    /// </summary>
    public T AttachedDataModel
    {
      get
      {
        return this.propAttachedDataModel;
      }

      set
      {
        this.propAttachedDataModel = value;
        this.ReadDataFromAttachedModel();
        this.OnPropertyChanged("AttachedDataModel");
      }
    }

    /// <summary>
    /// If the view model is currently reading data from the data model.
    /// </summary>
    public bool IsReadingDataModel
    {
      get
      {
        return this.propIsReadingDataModel;
      }

      private set
      {
        if (this.propIsReadingDataModel != value)
        {
          this.propIsReadingDataModel = value;
          this.OnPropertyChanged("AttachedDataModel");
        }
      }
    }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Called when setting the data model to null.
    /// </summary>
    private void OnNullingDataModel()
    {
      this.NullingDataModel?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Called when reading data model.
    /// </summary>
    private void OnReadingDataModel()
    {
      this.ReadingDataModel?.Invoke(this, new EventArgs<T>(this.AttachedDataModel));
    }

    /// <summary>
    /// Checks if the properties are equal and throws an exception if not.
    /// </summary>
    /// <param name="propDataModel">The Property of the Data Model.</param>
    /// <param name="propViewModel">The Property ot the View Model.</param>
    protected void CheckIfPropertiesMatch(PropertyInfo propDataModel, PropertyInfo propViewModel)
    {
      if (propViewModel.PropertyType != propDataModel.PropertyType)
      {
        string message = string.Format("The view model \"{0}\" does implement a different type for the property \"{1}\" than the data model to attach.", this.GetType().Name, propDataModel.Name);

        throw new Exception(message);
      }
    }

    /// <summary>
    /// Reads the data into the data model.
    /// </summary>
    private void ReadDataFromAttachedModel()
    {
      if (this.AttachedDataModel == null)
      {
        this.OnNullingDataModel();
        return;
      }

      this.IsReadingDataModel = true;

      foreach (PropertyInfo prop in this.AttachedDataModel.GetType().GetProperties())
      {
        PropertyInfo thisProp = this.GetType().GetProperty(prop.Name);
        if (thisProp == null)
        {
          continue;
        }

        this.CheckIfPropertiesMatch(prop, thisProp);
        object newValue = prop.GetValue(this.AttachedDataModel);
        thisProp.SetValue(this, newValue);
      }

      this.OnReadingDataModel();

      this.IsReadingDataModel = false;
    }

    /// <summary>
    /// Updates the property of the data model.
    /// </summary>
    /// <param name="propName">The name of the property to update.</param>
    /// <param name="value">The new value.</param>
    private void UpdateAttachedDataModel(string propName)
    {
      if (this.AttachedDataModel == null)
      {
        return;
      }

      PropertyInfo thisProp = this.GetType().GetProperty(propName);
      object value = thisProp.GetValue(this);

      PropertyInfo attachedProperty = this.AttachedDataModel.GetType().GetProperty(propName);
      if (attachedProperty != null)
      {
        attachedProperty.SetValue(this.AttachedDataModel, value);
      }
    }

    /// <summary>
    /// Is callled when a property of the instance was changed.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The arguments.</param>
    private void ViewModelWithAttachable_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      this.UpdateAttachedDataModel(e.PropertyName);
    }

    #endregion Methods
  }
}
