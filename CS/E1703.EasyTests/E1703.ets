#DropDB E1703
#Application E1703Win

*FillRecord
 Columns = Text
  Values = edited

*CompareScreenshot E1703_ListView_edited.png
*Action Save
*CompareScreenshot E1703_ListView_saved.png
*ProcessRecord
 Text = edited


*FillForm
 Text = 1
*CompareScreenshot E1703_DetailView_edited.png
*Action Save
*CompareScreenshot E1703_DetailView_saved.png