   M S B A M L   `   `   `  ����I  EPRN212_Project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:PRN212_Project.ViewsPRN212_Project.Views  5      E'clr-namespace:PRN212_Project.ViewModelsPRN212_Project.ViewModels  5      E'clr-namespace:PRN212_Project.ConvertersPRN212_Project.Converters  5      %    PRN212_Project.Views.ReportView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:PRN212_Project.Views     5      >
viewmodels'clr-namespace:PRN212_Project.ViewModels     5      >
converters'clr-namespace:PRN212_Project.Converters     5        � 	Resources  5      9   3PRN212_Project.Converters.BoolToVisibilityConverter   BoolToVisibilityConverter@   :PRN212_Project.Converters.InverseBoolToVisibilityConverter $  InverseBoolToVisibilityConverter6   0PRN212_Project.Converters.StatusToColorConverter  StatusToColorConverter  ReportItemStyle �� 
TargetType �� Property �� Value �� Triggers �� Binding �� Value P� Property P� Value��%4  &	        &	      &	 &     &	 9       5   
   6/     5   
   66     5   
   6,   �� 5   
   " ��6(   .�5      ��  � ��$ 0,5,0,5q�6'   �� 5       �@��Padding$ 10q�6(   �� 5       � ��$ 0,0,0,1q�60   �� 5       � �� �#E0E0E06,   �� 5       � ��
 �����6+    5      �� 5       6   �� *IsRead+	 False69   .a�5      ��  � �� �#EDF6FF63   5      P� 5       �@J�IsMouseOver$
 True��61   . �5      ��  � �� �#F5F5F563   5      5      5      .��5!      � 	 � RowDefinitions	 5"   
   �� 5#      $
��Auto �6   �� 5$      $��* �6   .+�5(   
   � $��0��6   $��0,50,0,0q�6   
 � ColumnDefinitions
 5)      �� 5*      $��* �6#   �� 5+      $
��Auto �6#   �� 5,      $
��Auto �6#   .+�5.      �� $��Reports��6   $��24(�6'   $
��Bold"�65   
���   �$��20,15q�6Z   �� 5/      $��1��6   $��Create��6%   $��10q�6`   $	��8,4q�6l   ��66   �� *ShowCreateFormCommand+y�50      �� *CanCreateReport+ �� 	Converter# [  �� 51      $��2��6   $��Refresh��6%   $��10q�6Z   $	��8,4q�6f   ��67   �� *RefreshCommand+6f   52      � 55   
   $��1��6   .+�57      �� $��0,0,1,0q�6   ���#E0E0E0���#F8F9FAy�58      �� *ShowCreateForm+# [ ._�59      � .+�5:      �� $
��Auto=�6#   .��5;      �� ��6'   �� *	Reports+��5<      �� 5=   "   .R�5>   &   �� #��[ 6-   ._�5?   *   �  � Cursor$
 Hand��6/   	 5@   .   �� 5A   2   $
��Auto �6@   �� 5B   2   $
��Auto �6@   �� 5C   2   $
��Auto �6@   �� 5D   2   $
��Auto �6@    J� InputBindings 5F   .   d� 5G   2    d� MouseAction$ 	LeftClicke�6?    �� Command 5H   ?   �� *DataContext.MarkAsReadCommand+ �� RelativeSource ��  �� AncestorType ItemsControl �� CommandParameter 5J   ?   �� .+�5L   .   �� $��0��69    �� Orientation$ 
Horizontal=�6F   .+�5M   2   �� $��SemiBold"�6S   ��6<   �� *Title+6S   A� 5N   2   $��10��6:   $��10��6E   ���#4285F4$��10,0,0,0q�6`   y�5O   :   �� *IsRead+# [ 5P   /   �� 5Q   .   $��1��68   $��12(�6a   $��0,5,0,0q�6o   ��6E   �� *
SenderName+6o   �� 5R   .   $��2��69   $ 
Horizontal=�6F   $��0,5,0,0q�6_   .+�5S   2   �� $��Status: ��6<   $��11(�6L   ���#6666Z   �� 5T   2   $��11(�6T   $��SemiBold"�6b   ��6<   �� *Status+��5U   <   �� *Status+# [ 5V   /   �� 5W   .   $��3��68   $��11(�5X   8   ���#666$��0,5,0,0q�6X   ��5W   E   �� *	CreatedAt+ �� StringFormat$ {0:dd MMM yyyy, HH:mm}��5X   X   5Y   +   5Z   '   5[   #   5]      5^      �� 5`      ���	#80FFFFFFy�5a      �� *	IsLoading+# [  ._�5b      �� $��
Loading...��6$   $��Center=�5c   $   $��Center=�5d   $   $��16(�5e   $   5f      5g      5h      �� 5k      $��0q�6   ���#E0E0E0
�������y�5l      �� *ShowCreateForm+# [  ._�5m      � $��20q�6   	 5n      �� 5o      $
��Auto �6(   �� 5p      $
��Auto �6(   �� 5q      $
��Auto �6(   �� 5r      $
��Auto �6(   �� 5s      $
��Auto �6(   �� 5t      $��* �6(   .+�5w      �� $��0��6    $��Create New Report��6-   $��24(�6F   $
��Bold"�6T   $��0,0,0,20q�6f   �� 5y      $��1��6    $��Title:��6-   $��SemiBold"�6;   $��0,0,0,5q�6Q   �� 5z      $��2��6   $��5q�6m   $��0,0,0,15q�6y   ��6+   �� *ReportTitle+ �� UpdateSourceTrigger$ PropertyChanged=�6y   �� 5|      $��3��6    $��Content:��6-   $��SemiBold"�6=   $��0,0,0,5q�6S   �� 5}      $��4��6    �� TextWrapping$
 Wrap=�5~       �� AcceptsReturn$
 True��62   $	��200��6G   $��5q�6T   $��0,0,0,20q�6`   ��5}   +   �� *ReportContent+$ PropertyChanged=�5~   `   �� 5�      $��5��6!   $ 
Horizontal=�6.   $��Right=�6G   .+�5�      �� $��Cancel��6!   $
��15,8q�6Z   $��0,0,10,0q�6i   
���������62   �� *CancelCreateCommand+6{   �� 5�      $��Submit��6!   $
��15,8q�6Z   ���#4285F4
���������62   �� *CreateReportCommand+6~   5�      5�      5�      �� 5�      
�������$��1q�5�      ���#E0E0E0y�5�      �� *IsDetailVisible+# [  ._�5�      � 	 5�      �� 5�      $
��Auto �6(   �� 5�      $��* �6(   .+�5�      �� $��0��6   ���#F8F9FA$��0,0,0,1q�6?   ���#E0E0E0._�5�      � 
 5�      �� 5�   "   $��* �63   �� 5�   "   $
��Auto �63   .+�5�      �� $��18(�6N   $��SemiBold"�6\   $��15,10q�6r   ��6(   �� *SelectedReport.Title+6r   �� 5�      $��1��6%   $��Close��65   $��10q�6l   $	��8,4q�6x   ��6E   �� *CloseDetailCommand+6x   5�      5�      �� 5�      $��1��6#   $
��Auto=�60   .��5�      �� $��15q�6%   .+�5�      �� $��SemiBold"�6S   ��6(   �� *SelectedReport.SenderName+6S   � 5�      $��0,5,0,15q�6#   
 5�   "   �� 5�   &   $
��Auto �67   �� 5�   &   $
��Auto �67   �� 5�   &   $
��Auto �67   �� 5�   &   $��* �67   .+�5�   "   �� $��0��6,   $��12(�5�   (   ���#666��5�   <   �� *SelectedReport.CreatedAt+$ {0:dd MMM yyyy, HH:mm}��5�   6   �� 5�   "   $��1��6,   $�� | Status: ��6<   $��12(�6O   ���#666$��5,0,0,0q�6o   �� 5�   "   $��2��6,   $��12(�5�   %   $��SemiBold"�63   ��5�   <   �� *SelectedReport.Status+��5�   %   �� *SelectedReport.Status+# [ y�5�   %   �� *CanChangeReportStatus+# [ �� 5�   "   $��2��6-   $ 
Horizontal=�6=   y�5�   %   �� *CanChangeReportStatus+# [  .+�5�   &   �� $	��120��6/    �� SelectedItem 6;   �� *SelectedReport.Status+��6j   �� *ReportStatusOptions+�� 5�   &   $��Update��6-   $��10,0,0,0q�5�   &   $	��8,2q�68   ���#4285F4
���������5�   >   �� *UpdateReportStatusCommand+5�   [   5�   #   5�      �� 5�      $
��Wrap=�6G   ��6(   �� *ReportContent+6G   5�      5�      5�      5�      5�      5�      5�      