
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;


namespace AdminPanel.CommonRepo
{

    public static class CommonDropdown
    {

        #region Status Dropdowns

        public static List<SelectListItem> GetStatusDropdown()
        {
          List<SelectListItem> selectListItems = new List<SelectListItem>();

            SelectListItem Active = new SelectListItem();

            Active.Text = "Active";
            Active.Value = "A";


            selectListItems.Add(Active);


            SelectListItem Delete = new SelectListItem();

            Delete.Text = "Delete";
            Delete.Value = "D";


            selectListItems.Add(Delete);



            SelectListItem EDIT = new SelectListItem();

            EDIT.Text = "Locked";
            EDIT.Value = "L";


            selectListItems.Add(EDIT);

            return selectListItems;
        }
        #endregion



        #region Calculation Dropdown 
        public static List<SelectListItem> GetCalculationDropdown()
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();

            SelectListItem Addition = new SelectListItem();

            Addition.Text = "Addition";
            Addition.Value ="+";


            selectListItems.Add(Addition);


            SelectListItem Subtraction = new SelectListItem();

            Subtraction.Text = "Subtraction";
            Subtraction.Value = "-";


            selectListItems.Add(Subtraction);



            SelectListItem Division = new SelectListItem();

            Division.Text = "Division";
            Division.Value = "/";


            selectListItems.Add(Division);

            SelectListItem Multiplication = new SelectListItem();

            Multiplication.Text = "Multiplication";
            Multiplication.Value = "*";


            selectListItems.Add(Division);

            return selectListItems;
        }
        #endregion

       


        #region TransPort Type Dropdown
        public static List<SelectListItem> GetTransport_Type_Dropdown()
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();

            SelectListItem Air = new SelectListItem();

            Air.Text = "Air";
            Air.Value = "Air";


            selectListItems.Add(Air);


            SelectListItem Ocean = new SelectListItem();

            Ocean.Text = "Ocean";
            Ocean.Value = "Ocean";


            selectListItems.Add(Ocean);



            SelectListItem Road = new SelectListItem();

            Road.Text = "Road";
            Road.Value = "Road";


            selectListItems.Add(Road);


            SelectListItem Train = new SelectListItem();

            Train.Text = "Train";
            Train.Value = "Train";


            selectListItems.Add(Train);

            return selectListItems;
        }
        #endregion

        public static List<SelectListItem> GetCarrier_Type_Dropdown()
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();

            SelectListItem Core_Carrier = new SelectListItem();

            Core_Carrier.Text = "Core Carrier";
            Core_Carrier.Value = "Core_Carrier";


            selectListItems.Add(Core_Carrier);


            SelectListItem Non_Core_Carrier = new SelectListItem();

            Non_Core_Carrier.Text = "Non Core Carrier";
            Non_Core_Carrier.Value = "Non_Core_Carriercean";


            selectListItems.Add(Non_Core_Carrier);


            return selectListItems;
        }

        public static List<SelectListItem> GetYesNoDropdown()
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();

            SelectListItem Yes = new SelectListItem();

            Yes.Text = "Yes";
            Yes.Value = "Yes";


            selectListItems.Add(Yes);


            SelectListItem No = new SelectListItem();

            No.Text = "No";
            No.Value = "No";


            selectListItems.Add(No);


            return selectListItems;
        }


        public static List<SelectListItem> GetImportExportDropdown()
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();

            SelectListItem Import = new SelectListItem();

            Import.Text = "Import";
            Import.Value = "Import";


            selectListItems.Add(Import);


            SelectListItem Export = new SelectListItem();

            Export.Text = "Export";
            Export.Value = "Export";


            selectListItems.Add(Export);



            SelectListItem NotApplicable = new SelectListItem();

            NotApplicable.Text = "Not Applicable";
            NotApplicable.Value = "NotApplicable";


            selectListItems.Add(NotApplicable);

            return selectListItems;
        }
        public static List<SelectListItem> GetFull_GroupageDropdown()
        {
            List<SelectListItem> SelectListItem = new List<SelectListItem>();

            SelectListItem Full_Groupage = new SelectListItem();

            Full_Groupage.Text = "Full";
            Full_Groupage.Value = "Full";


            SelectListItem.Add(Full_Groupage);


            SelectListItem Groupage = new SelectListItem();

            Groupage.Text = "Groupage";
            Groupage.Value = "Groupage";


            SelectListItem.Add(Groupage);



            SelectListItem NotApplicable = new SelectListItem();

            NotApplicable.Text = "Not Applicable";
            NotApplicable.Value = "NotApplicable";


            SelectListItem.Add(NotApplicable);

            return SelectListItem;
        }

        public static List<SelectListItem> Get_EmployeeStatusDropdown()
        {
            List<SelectListItem> SelectListItem = new List<SelectListItem>();

            SelectListItem Employed = new SelectListItem();

            Employed.Text = "Employed ";
            Employed.Value = "Employed ";


            SelectListItem.Add(Employed);


            SelectListItem Rejoined = new SelectListItem();

            Rejoined.Text = "Rejoined ";
            Rejoined.Value = "Rejoined ";


            SelectListItem.Add(Rejoined);



            SelectListItem Resigned = new SelectListItem();

            Resigned.Text = "Resigned ";
            Resigned.Value = "Resigned ";


            SelectListItem.Add(Resigned);


            SelectListItem Terminated = new SelectListItem();

            Terminated.Text = "Terminated";
            Terminated.Value = "Terminated";


            SelectListItem.Add(Terminated);



            SelectListItem Transferred = new SelectListItem();

            Transferred.Text = "Transferred";
            Transferred.Value = "Transferred";


            SelectListItem.Add(Transferred);

            SelectListItem Contract = new SelectListItem();

            Contract.Text = "Contract";
            Contract.Value = "Contract";


            SelectListItem.Add(Contract);

            return SelectListItem;
        }
    }
}
