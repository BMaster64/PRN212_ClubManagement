   M S B A M L   `   `   `  ����I  EPRN212_Project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:PRN212_Project.ViewsPRN212_Project.Views  5      E'clr-namespace:PRN212_Project.ConvertersPRN212_Project.Converters  5      #    PRN212_Project.Views.ClubView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:PRN212_Project.Views     5      >
converters'clr-namespace:PRN212_Project.Converters     5        � 	Resources  5
      7   1PRN212_Project.Converters.UserTypeToRoleConverter   UserTypeToRoleConverter9   3PRN212_Project.Converters.BoolToVisibilityConverter  BoolToVisibilityConverter@   :PRN212_Project.Converters.InverseBoolToVisibilityConverter $  InverseBoolToVisibilityConverter��%W   &	        &	      &	 &       5   
   6-     5   
   6/     5   
   66   5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��	10,50,0,0q�6.   .+�5      �� $��Club Management��6   $��24(�6/   $
��Bold"�6=   $��0,0,20,0q�6O   �� 5      $��Export to Excel��6     NavButtonStyle#��[ 5      $	��120��5      $��30��6   $��10,0,0,0q�5      
��� � ���5      �� *ExportToExcelCommand+5      5      &   System.Windows.Controls.DataGrid  5!   
   $��1��6      AutoGenerateColumns$ False��5"         
IsReadOnly$
 True��6/   $��10q�5#      ��5!       �� *Clubs+y�5#      �� *ShowEditClubForm+ �� 	Converter# [    Columns 5$      0  *System.Windows.Controls.DataGridTextColumn  5%      ,  &System.Windows.Controls.DataGridColumn   Header$ Club ID��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6Q   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 66   �� *ClubId+6Q     5&      $ 	Club Name��6%   $ * 6U   	 68   �� *
ClubName+6U   4	  .System.Windows.Controls.DataGridTemplateColumn	  5'      $ Actions��6)   $ * 6:   
 	  CellTemplate
 5(      �� 5)      .R�5*      �� $ 
Horizontal=�6)   .+�5+   "   �� $
��Edit��6)   #��[ 5/   )   
��� d �$��14,0,5,0q�51   )   $��50��6;   ��5,   )   �� *DataContext.EditClubCommand+ �� RelativeSource ��  �� AncestorType DataGrid��5.   )   �� 51   ;   �� 53   "   $��Delete��6)   #��[ 57   )   
���\\��$��0,0,5,0q�59   )   $��50��6:   ��54   )   �� *DataContext.DeleteClubCommand+ ��  DataGrid��56   )   �� 59   :   �� 5;   "   $��Manage��6)   #��[ 5?   )   
����  �$��0,0,5,0q�5A   (   $��70��69   ��5<   )   �� *DataContext.ManageClubCommand+ ��  DataGrid��5>   )   �� 5A   9   5B      5C      5E      5G      � 5J   
   $��1��6   $��10q�6   y�5K      �� *ShowEditClubForm+# [ .+�5L      �� $	��400��6   .+�5M      �� $��	Edit Club��6   $��20(�6-   $
��Bold"�6;   $��0,0,0,10q�6M   �� 5O      $��
Club Name:��6   �� 5P      $��0,5,0,10q�6A   ��6   �� *SelectedClub.ClubName+6A   �� 5R      $ 
Horizontal=�6   $��Right=�66   .+�5S      �� $��Cancel��6   #��[ 5U      $	��100��5V      $��30��6)   $��0,0,10,0q�5W      
���  ����5T      �� *CancelEditCommand+5X      �� 5Z      $
��Save��6   #��[ 5\      $	��100��5]      $��30��6)   
��� d ���5[      �� *SaveEditClubCommand+5^      5_      5`      5a      5b      5c      