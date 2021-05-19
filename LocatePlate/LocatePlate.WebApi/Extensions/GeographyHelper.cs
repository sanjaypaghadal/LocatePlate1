//using LocatePlate.Infrastructure.Geography;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Reflection;

//namespace LocatePlate.WebApi.Extensions
//{
//    public static class GeographyHelper
//    {

//        public static void StateDropdown()
//        {
//            // Array itemValues = System.Enum.GetValues<Cate>(typeof(StateProvince));
//            var itemNames = System.Enum.GetNames(typeof(StateProvince));


//            Array values = Enum.GetValues(typeof(StateProvince));


//            var items = new List<SelectListItem>();


//            for (int i = 0; i <= values.Length - 1; i++)
//            {
//                object v = values.GetValue(i);
//                items.Add(new SelectListItem
//                {
//                    Text = GetEnumDescription(values.GetValue(i)),
//                    Value = Convert.ToString(v)
//                    //Selected = value.Equals(metadata.Model)
//                });
//            }
//            //foreach (var item in collection)
//            //{

//            //}
//        }


//        private static readonly SelectListItem[] SingleEmptyItem = new[] { new SelectListItem { Text = "", Value = "" } };

//        public static string GetEnumDescription<TEnum>(TEnum value)
//        {
//            FieldInfo fi = value.GetType().GetField(value.ToString());

//            DescriptionAttribute[] descriptionAttributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
//            CategoryAttribute[] attributes = (CategoryAttribute[])fi.GetCustomAttributes(typeof(CategoryAttribute), false);
//            Attribute[] attribtes = (Attribute[])fi.GetCustomAttributes(typeof(Attribute), false);



//            if ((descriptionAttributes != null) && (descriptionAttributes.Length > 0))
//                return descriptionAttributes[0].Description;
//            else
//                return value.ToString();
//        }
//    }
//}
