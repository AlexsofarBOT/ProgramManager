﻿using System;
using System.Collections.Generic;
using ProgramManager.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace ProgramManager.ViewModels
{
    public partial class MainViewModel
    {
        /// <summary>
        /// This method to save a list current state before will begin search. 
        /// </summary>
        private void FixStatePackage()
        {
            if (string.IsNullOrEmpty(_filterPackages))
                _storage = Packages;
            else
                Packages = _storage;
        }
        /// <summary>
        /// Repacks object for normal data output of the current package.
        /// </summary>
        /// <param name="obj">Data of the current pack.</param>
        private void SetCurrentPackage(PackageModel obj)
        {
            List<DataPanelModel> current = new List<DataPanelModel>();

            Type classType = typeof(PackageModel);

            PropertyInfo[] properties = classType.GetProperties();

            foreach (var property in properties)
            {
                if (property.GetValue(obj) != null)
                {
                    current.Add(new DataPanelModel
                        { Label = "Имя:", Field = obj.GetType().GetProperty(property.Name)?.GetValue(obj).ToString() });
                }
            }
            _currentPackage = current;
        }
        /// <summary>
        /// Calculates packages number which correspond to the Tag and shows number in each tag.
        /// </summary>
        private void CalculateByTag()
        {
            List<CategoryModel> tags = CategoryAccess.GetSubcategories();
            int total = 0, count = 0;

            foreach (var category in tags)
            {
                total += _instancePackages.Count(t => t.TagName.Contains(category.TagName));
                tags[count].Count = total;
                total = 0;
                count++;
            }
            Tags = new ObservableCollection<CategoryModel>(tags);
        }
        /// <summary>
        /// Calculates packages number which correspond to the Category and shows number in item "All".
        /// </summary>
        private void CalculateByCategory()
        {
            List<CategoryModel> categories = CategoryAccess.GetCategories();
            int total = 0, count = 0;

            foreach (var category in categories)
            {
                total += _instancePackages.Count(t => t.Category == category.CategoryName);
                Category[count].Count = total;
                total = 0;
                count++;
            }
        }
    }
}
