   M S B A M L   `   `   `  ����I  EPRN212_Project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:PRN212_Project.ViewsPRN212_Project.Views  5      E'clr-namespace:PRN212_Project.ViewModelsPRN212_Project.ViewModels  5      E'clr-namespace:PRN212_Project.ConvertersPRN212_Project.Converters  5      +    %PRN212_Project.Views.NotificationView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:PRN212_Project.Views     5      >
viewmodels'clr-namespace:PRN212_Project.ViewModels     5      >
converters'clr-namespace:PRN212_Project.Converters     5        � 	Resources  5      9   3PRN212_Project.Converters.BoolToVisibilityConverter   BoolToVisibilityConverter@   :PRN212_Project.Converters.InverseBoolToVisibilityConverter $  InverseBoolToVisibilityConverter  NotificationItemStyle �� 
TargetType �� Property �� Value �� Triggers �� Binding �� Value P� Property P� Value��%  &	        &	      &	 &       5   
   6/     5   
   66   �� 5   
   " ��6.   .�5      ��  � ��$ 0,5,0,5q�6'   �� 5       �@��Padding$ 10q�6(   �� 5       � ��$ 0,0,0,1q�60   �� 5       � �� �#E0E0E06,   �� 5       � ��
 �����6+    5      �� 5       6   �� *IsRead+	 False69   .a�5      ��  � �� �#EDF6FF63   5      P� 5       �@J�IsMouseOver$
 True��61   . �5      ��  � �� �#F5F5F563   5      5      5      .��5       � 	 � RowDefinitions	 5!   
   �� 5"      $
��Auto �6   �� 5#      $��* �6   .+�5'   
   � $��0��6   $��0,50,0,0q�6   
 � ColumnDefinitions
 5(      �� 5)      $��* �6#   �� 5*      $
��Auto �6#   �� 5+      $
��Auto �6#   .+�5-      �� $��Notifications��6   $��24(�6-   $
��Bold"�6;   
���   �$��20,15q�6`   �� 5.      $��1��6   $��Create��6%   $��10q�6`   $	��8,4q�6l   ��66   �� *ShowCreateFormCommand+y�5/      �� *CanCreateNotification+ �� 	Converter# [  �� 50      $��2��6   $��Refresh��6%   $��10q�6Z   $	��8,4q�6f   ��67   �� *RefreshCommand+6f   51      � 54   
   $��1��6   .+�56      �� $��0,0,1,0q�6   ���#E0E0E0���#F8F9FAy�57      �� *ShowCreateForm+# [ ._�58      � .+�59      �� $
��Auto=�6#   .��5:      �� ��6'   �� *Notifications+��5;      �� 5<   "   .R�5=   &   �� #��[ 6-   ._�5>   *   �  � Cursor$
 Hand��6/   	 5?   .   �� 5@   2   $
��Auto �6@   �� 5A   2   $
��Auto �6@   �� 5B   2   $
��Auto �6@   
 5D   ,   �� 5E   .   $��* �6?   �� 5F   .   $
��Auto �6?    J� InputBindings 5H   .   d� 5I   2    d� MouseAction$ 	LeftClicke�6?    �� Command 5J   ?   �� *DataContext.MarkAsReadCommand+ �� RelativeSource ��  �� AncestorType ItemsControl �� CommandParameter 5L   ?   �� .+�5N   ,   �� $��0��67   $��0��6D    �� Orientation$ 
Horizontal=�6T   .+�5O   .   �� $��SemiBold"�6O   ��68   �� *Title+6O   A� 5P   .   $��10��66   $��10��6A   ���#4285F4$��10,0,0,0q�6\   y�5Q   6   �� *IsRead+# [ 5R   -   �� 5S   ,   $��2��66   $��0��6C   $��11(�5U   6   ���#666$��0,5,0,0q�6V   ��5T   6   �� *	CreatedAt+ �� StringFormat$ {0:dd MMM yyyy, HH:mm}��5U   V   �� 5V   ,   $��0��67   $��1��6D   $ 
Horizontal=�6T   y�5W   9   �� *#!DataContext.CanCreateNotification+ ��  ItemsControl# [  .+�5Z   .   �� $
��Edit��65   $��0,0,5,0q�5^   5   $	��5,2q�5_   5   ��5[   5   �� *DataContext.StartEditCommand+ ��  ItemsControl��5]   5   �� 5_   5   �� 5`   .   $��Delete��65   $	��5,2q�5d   5   
���\\��
���������5a   5   �� *'%DataContext.DeleteNotificationCommand+ ��  ItemsControl��5c   5   �� 5f   5   5g   -   5h   +   5i   '   5j   #   5l      5m      �� 5p      ���	#80FFFFFFy�5q      �� *	IsLoading+# [  ._�5r      �� $��
Loading...��6$   $��Center=�5s   $   $��Center=�5t   $   $��16(�5u   $   5v      5w      5x      �� 5{      $��0q�6   ���#E0E0E0
�������y�5|      �� *ShowCreateForm+# [  ._�5}      � $��20q�6   	 5~      �� 5      $
��Auto �6(   �� 5�      $
��Auto �6(   �� 5�      $
��Auto �6(   �� 5�      $
��Auto �6(   �� 5�      $
��Auto �6(   �� 5�      $��* �6(   .+�5�      �� $��0��6    $��Create New Notification��6-   $��24(�6L   $
��Bold"�6Z   $��0,0,0,20q�6l   �� 5�      $��1��6    $��Title:��6-   $��SemiBold"�6;   $��0,0,0,5q�6Q   �� 5�      $��2��6   $��5q�6s   $��0,0,0,15q�6   ��6+   �� *NotificationTitle+ �� UpdateSourceTrigger$ PropertyChanged=�6   �� 5�      $��3��6    $��Content:��6-   $��SemiBold"�6=   $��0,0,0,5q�6S   �� 5�      $��4��6    �� TextWrapping$
 Wrap=�5�       �� AcceptsReturn$
 True��62   $	��200��6G   $��5q�6T   $��0,0,0,20q�6`   ��5�   +   �� *NotificationContent+$ PropertyChanged=�5�   `   �� 5�      $��5��6!   $ 
Horizontal=�6.   $��Right=�6G   .+�5�      �� $��Cancel��6!   $
��15,8q�6Z   $��0,0,10,0q�6i   
���������62   �� *CancelCreateCommand+6{   �� 5�      $
��Send��6!   $
��15,8q�6^   ���#4285F4
���������60   �� *CreateNotificationCommand+6�   5�      5�      5�      �� 5�      $��0q�6   ���#E0E0E0
�������y�5�      �� *
IsEditMode+# [  ._�5�      � $��20q�6   	 5�      �� 5�      $
��Auto �6    �� 5�      $
��Auto �6    �� 5�      $
��Auto �6    �� 5�      $
��Auto �6    �� 5�      $
��Auto �6    �� 5�      $��* �6    .+�5�      �� $��0��6   $��Edit Notification��6'   $��24(�6@   $
��Bold"�6N   $��0,0,0,20q�6`   �� 5�      $��1��6   $��Title:��6'   $��SemiBold"�65   $��0,0,0,5q�6K   �� 5�      $��2��6   $��5q�6m   $��0,0,0,15q�6y   ��6%   �� *NotificationTitle+$ PropertyChanged=�6y   �� 5�      $��3��6   $��Content:��6'   $��SemiBold"�67   $��0,0,0,5q�6M   �� 5�      $��4��6   $
 Wrap=�5�      $
 True��6,   $	��200��6A   $��5q�6N   $��0,0,0,20q�6Z   ��5�   %   �� *NotificationContent+$ PropertyChanged=�5�   Z   �� 5�      $��5��6   $ 
Horizontal=�6(   $��Right=�6A   .+�5�      �� $��Cancel��6   $
��15,8q�5�      $��0,0,10,0q�5�      
���������5�      �� *CancelCreateCommand+5�      �� 5�      $��Update��6   $
��15,8q�5�      ���#4285F4
���������5�      �� *UpdateNotificationCommand+5�      5�      5�      5�      �� 5�      
�������$��1q�5�      ���#E0E0E0y�5�      �� *IsDetailVisible+# [  ._�5�      � 	 5�      �� 5�      $
��Auto �6(   �� 5�      $��* �6(   .+�5�      �� $��0��6   ���#F8F9FA$��0,0,0,1q�6?   ���#E0E0E0._�5�      � 
 5�      �� 5�   "   $��* �63   �� 5�   "   $
��Auto �63   .+�5�      �� $��18(�6T   $��SemiBold"�6b   $��15,10q�6x   ��6(   �� *SelectedNotification.Title+6x   �� 5�      $��1��6%   $��Close��65   $��10q�6l   $	��8,4q�6x   ��6E   �� *CloseDetailCommand+6x   5�      5�      �� 5�      $��1��6#   $
��Auto=�60   .��5�      �� $��15q�6%   .+�5�      �� $��SemiBold"�6Y   ��6(   �� *!SelectedNotification.SenderName+6Y   �� 5�      $��12(�5�   (   ���#666$��0,5,0,15q�6H   ��5�   (   �� * SelectedNotification.CreatedAt+$ {0:dd MMM yyyy, HH:mm}��5�   H   �� 5�      $
��Wrap=�6M   ��6(   �� *NotificationContent+6M   5�      5�      5�      5�      5�      5�      5�      