namespace Spotify.PrismExtensions
{
  using System;
  using System.ComponentModel;
  using System.Reflection;

  using Microsoft.Practices.Prism.Mvvm;

  /// <summary>A view model with attachable data model.</summary>
  /// <typeparam name="T">The Type of the data model.</typeparam>
  internal class ViewModelWithAttachable<T> : ViewModel
  {
    #region Fields

    /// <summary>The property attached data models value.</summary>
    private T propAttachedDataModel;

    /// <summary>The property is reading data models value.</summary>
    private bool propIsReadingDataModel;

    #endregion

    #region Constructors

    /// <summary>Initializes a new instance of the <see cref="ViewModelWithAttachable{T}" /> class.</summary>
    public ViewModelWithAttachable()
    {
      this.PropertyChanged += this.ViewModelWithAttachablePropertyChanged;
    }

    #endregion

    #region Events

    /// <summary>Occurs when nulling the data model.</summary>
    public event EventHandler<EventArgs> NullingDataModel;

    /// <summary>Occurs when reading the data model.</summary>
    public event EventHandler<EventArgs<T>> ReadingDataModel;

    #endregion

    #region Properties

    /// <summary>Gets or sets the attached data model. This should be a POCO class which will be automatically updated if a property linked to it changes. To update the ViewModel attach the model again.</summary>
    public T AttachedDataModel
    {
      get { return this.propAttachedDataModel; }

      set
      {
        this.propAttachedDataModel = value;
        this.ReadDataFromAttachedModel();
        this.OnPropertyChanged("AttachedDataModel");
      }
    }

    /// <summary>Gets or sets a value indicating whether the view model is currently reading data from the data model.</summary>
    public bool IsReadingDataModel
    {
      get { return this.propIsReadingDataModel; }
      set { this.SetProperty(ref this.propIsReadingDataModel, value); }
    }

    #endregion

    #region Methods

    /// <summary>Checks if the properties are equal and throws an exception if not.</summary>
    /// <param name="propDataModel">The Property of the Data Model.</param>
    /// <param name="propViewModel">The Property ot the View Model.</param>
    protected void CheckIfPropertiesMatch(PropertyInfo propDataModel, PropertyInfo propViewModel)
    {
      if (propViewModel.PropertyType != propDataModel.PropertyType)
      {
        var message = string.Format("The view model \"{0}\" does implement a different type for the property \"{1}\" than the data model to attach.", this.GetType().Name, propDataModel.Name);

        throw new Exception(message);
      }
    }

    /// <summary>Called when setting the data model to null.</summary>
    private void OnNullingDataModel()
    {
      this.NullingDataModel?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>Called when reading data model.</summary>
    private void OnReadingDataModel()
    {
      this.ReadingDataModel?.Invoke(this, new EventArgs<T>(this.AttachedDataModel));
    }

    /// <summary>Reads the data into the data model.</summary>
    private void ReadDataFromAttachedModel()
    {
      if (this.AttachedDataModel == null)
      {
        this.OnNullingDataModel();
        return;
      }

      this.IsReadingDataModel = true;

      foreach (var prop in this.AttachedDataModel.GetType().GetProperties())
      {
        var thisProp = this.GetType().GetProperty(prop.Name);
        if (thisProp == null)
        {
          continue;
        }

        this.CheckIfPropertiesMatch(prop, thisProp);
        var newValue = prop.GetValue(this.AttachedDataModel);
        thisProp.SetValue(this, newValue);
      }

      this.OnReadingDataModel();

      this.IsReadingDataModel = false;
    }

    /// <summary>Updates the property of the data model.</summary>
    /// <param name="propName">The name of the property to update.</param>
    private void UpdateAttachedDataModel(string propName)
    {
      if (this.AttachedDataModel == null)
      {
        return;
      }

      var thisProp = this.GetType().GetProperty(propName);
      var value = thisProp.GetValue(this);

      var attachedProperty = this.AttachedDataModel.GetType().GetProperty(propName);
      if (attachedProperty != null)
      {
        attachedProperty.SetValue(this.AttachedDataModel, value);
      }
    }

    /// <summary>Is called when a property of the instance was changed.</summary>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The arguments.</param>
    private void ViewModelWithAttachablePropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      this.UpdateAttachedDataModel(e.PropertyName);
    }

    #endregion
  }
}