   M S B A M L   `   `   `  ����I  EPRN212_Project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:PRN212_Project.ViewsPRN212_Project.Views  5      E'clr-namespace:PRN212_Project.ConvertersPRN212_Project.Converters  5      %    PRN212_Project.Views.MemberView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:PRN212_Project.Views     5      >
converters'clr-namespace:PRN212_Project.Converters     5        � 	Resources  5
      7   1PRN212_Project.Converters.UserTypeToRoleConverter   UserTypeToRoleConverter9   3PRN212_Project.Converters.BoolToVisibilityConverter  BoolToVisibilityConverter@   :PRN212_Project.Converters.InverseBoolToVisibilityConverter $  InverseBoolToVisibilityConverter5   /PRN212_Project.Converters.BoolToStringConverter  BoolToStringConverter4   .PRN212_Project.Converters.InverseBoolConverter  InverseBoolConverter6   0PRN212_Project.Converters.BoolToCommandConverter  BoolToCommandConverterC   =PRN212_Project.Converters.RolePermissionToVisibilityConverter ' #RolePermissionToVisibilityConverter��%�   &	        &	      &	 &     &	 9     &	 L     &	 _     &	 r       5   
   6-     5   
   6/     5   
   66     5   
   6+     5   
   6*     5   
   6,     5   
   69   5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��	10,50,0,0q�6.   .+�5      �� $��Member Management��6   $��24(�61   $
��Bold"�6?   $��0,0,20,0q�6Q   �� 5      $��
Add Member��6     NavButtonStyle#��[ 5      $	��100��5       $��30��6   
����  ���5      �� *ShowCreateFormCommand+y�5!      �� *CanAddMembers+ �� 	Converter# [ 6h   �� 5"      $��Export to Excel��6   #��[ 5$      $	��120��5%      $��30��6   $��10,0,0,0q�5&      
��� � ���5#      �� *ExportToExcelCommand+5'      5(      &   System.Windows.Controls.DataGrid  5+   
   $��1��6      AutoGenerateColumns$ False��5,         
IsReadOnly$
 True��6'   $��10q�5-      ��5+       �� *	Members+y�5-      �� *ShowCreateMemberForm+# [    Columns 5.      0	  *System.Windows.Controls.DataGridTextColumn	  5/      ,
  &System.Windows.Controls.DataGridColumn 
  Header$ ID��6%    
  Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6O   1  +System.Windows.Controls.DataGridBoundColumn	   Binding	 61   �� *	StudentId+6O   	  50      $ 	Full Name��6%   $ * 6U   	 68   �� *
FullName+6U   	  51      $ Date Joined��6%   $ * 6w   	 6:   �� *	CreatedAt+
 �� StringFormat$
 {0:MM/dd/yyyy}��6w   	  52      $
 Role��6%   $ * 6�   	 63   �� *RoleId+# [  6�   4  .System.Windows.Controls.DataGridTemplateColumn  53      $ Actions��6)   $ * 6:      CellTemplate 54      �� 55      .R�56      �� $ 
Horizontal=�6)   y�57   "   `� 58   &    `� 	Converter# [ 63   .1�59   *   ��  �� Path$ RoleId�62   �� 5:   *   $$ DataContext.CurrentUser.RoleId�62    �� RelativeSource 5;   "   ��  �� AncestorType DataGrid5<   '   .+�5>   "   �� $
��Edit��6)   #��[ 5B   !   
��� d �$��14,0,5,0q�5D   !   $��50��63   ��5?   !   �� *DataContext.EditMemberCommand+ ��  DataGrid��5A   !   �� 5D   3   �� 5F   "   $��Delete��6)   #��[ 5J   !   
���\\��$��0,0,14,0q�5L   !   $��50��63   ��5G   !   �� *!DataContext.DeleteMemberCommand+ ��  DataGrid��5I   !   �� 5L   3   5M      5N      5P      5R      � 5U   
   $��1��6   $��10q�6   y�5V      �� *ShowCreateMemberForm+# [ .+�5W      �� $	��400��6   .+�5Y      �� $��20(�5[      $
��Bold"�6    $��0,0,0,10q�62   ��5Y      �� *	IsEditing+# [  �� ConverterParameter$# Edit Member|Create New Member��5[   2   �� 5^      $��Student ID:��6   �� 5_      $��0,5,0,10q�65   ��6   �� *	StudentId+{�5`      �� *	IsEditing+# [ �� 5b      $��
Full Name:��6   �� 5c      $��0,5,0,10q�64   ��6   �� *
Fullname+64   �� 5e      $��	Username:��6   �� 5f      $��0,5,0,10q�64   ��6   �� *
Username+64   �� 5h      $��	Password:��6   �� 5i      $ Vertical=�6   .+�5j      G� -   5         G�Name$ PasswordBox��5j   "   $��0,5,0,0q�67   �� 5k      $*��$Leave blank to keep current password��6    $��Italic$�5m      $��12(�6+   $��0,2,0,10q�69   y�5l      �� *	IsEditing+# [ 5m   9   5n      �� 5p      $��
User Type:��6   �� 5q      $��0,5,0,20q�5s      ��5q      �� *AvailableUserTypes+ �� SelectedItem 5r      �� *SelectedUserType+��5t      �� 5u      .R�5v      �� ��6(   �� # [  5w      5y      �� 5|      $ 
Horizontal=�6   $��Right=�66   .+�5}      �� $��Cancel��6   #��[ 5      $	��100��6=   $��30��6I   $��0,0,10,0q�6U   
���  ����5~      �� *CancelCreateCommand+y�5�      �� *	IsEditing+# [ 6s   �� 5�      $��Cancel��6   #��[ 5�      $	��100��6=   $��30��6I   $��0,0,10,0q�6U   
���  ����5�      �� *CancelEditCommand+y�5�      �� *	IsEditing+# [ 6l   �� 5�      $��Create��6   #��[ 5�      $	��100��6=   $��30��6I   
��� d ���5�      �� *CreateMemberCommand+y�5�      �� *	IsEditing+# [ 6s   �� 5�      $
��Save��6   #��[ 5�      $	��100��6=   $��30��6I   
��� d ���5�      �� *SaveEditCommand+y�5�      �� *	IsEditing+# [ 6l   5�      5�      5�      5�      5�      