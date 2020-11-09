# Spendi
A retail store-front application that tracks inventory, prices, and descriptions of products. A user may authenticate through a portal and purchase products through the sales page.

# Installing
Open the solution in Visual Studio, there are multiple projects in this solution so you may need to add them `Spendi.sln > Add > Existing Project > ...`

Navigate to the drop down next to Start and select `Set Startup Projects` > `Multiple Startup Projects` and set SpendiData, SpendiDataManager, and SpendiDesktopUI to Start, leave the rest at None.

If you do not have `Data Storage and Processing` installed in your Visual Studio installation open the Visual Studio Installer and add them to your installation. You should publish the database and make note of the connection string. Right click `SpendiData > Publish > Target database connection > Edit > <default> MSSSQL` Save the connection string and open and replace SpendiData.publish.xml with the new location.

# Running the tests
Open the application and log in with the default credentials. Select an item to view its description and adjust the quantity with the text box, add or remove from cart with the buttons below. Click checkout to clear cart and see that the quantities of the items in store have adjusted.


# Authors
  - Vincent Andrea
  
# License
This project is licensed under the GNU General Public License v3.0 - see the LICENSE.md file for details
