   M S B A M L   `   `   `  ����I  EPRN212_Project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullE'clr-namespace:PRN212_Project.ConvertersPRN212_Project.Converters  5   	   1    +PRN212_Project.Views.EventRegistrationsView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   >
converters'clr-namespace:PRN212_Project.Converters     5   	     � Title$  Event Registrations��5   	   $	��450��5   	   $	��600��5   	    � WindowStartupLocation$ CenterScreen=�5   	    � 	Resources 5	      9   3PRN212_Project.Converters.BoolToVisibilityConverter   BoolToVisibilityConverter@   :PRN212_Project.Converters.InverseBoolToVisibilityConverter $  InverseBoolToVisibilityConverter��%:   &	        &	        5
   
   6/     5   
   66   5      .��5      � $��20q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   $��0,0,0,20q�6"   .+�5      �� $��20(�5      $
��Bold"�5      $��0,0,0,5q�5      ��5      �� *SelectedEvent.EventName+5      �� 5      �� 5      $�Date: ��6    �� 5      �6   �� *SelectedEvent.StartTime+ �� StringFormat$ {0:dd MMM yyyy, HH:mm}��5      �� 5      �� 5      $�
Location: ��6    �� 5       �6   �� *SelectedEvent.Location+5!      5"      &   System.Windows.Controls.DataGrid  5%   
   $��1��6      AutoGenerateColumns$ False��5'         
IsReadOnly$
 True��5(      ��5&      �� *RegisteredMembers+   Columns 5)      0  *System.Windows.Controls.DataGridTextColumn  5*      ,  &System.Windows.Controls.DataGridColumn   Header$ 
Student ID��6%   	   Width5  /System.Windows.Controls.DataGridLengthConverter$		 100 5,   %   1  +System.Windows.Controls.DataGridBoundColumn
   Binding
 5+   %   �� *	StudentId+5,   %     5-      $ 	Full Name��6%   $	 * 5/   %   
 5.   %   �� *
FullName+5/   %     50      $ Username��6%   $	 * 52   %   
 51   %   �� *
Username+52   %     53      $ Status��6%   $		 100 55   %   
 54   %   �� *RegistrationStatus+55   %     56      $ Registration Time��6%   $		 150 58   %   
 57   %   �� *RegistrationTime+$ {0:dd MMM yyyy, HH:mm}��58   %   5:      � 5=   
   $��1��6   ���	#80FFFFFFy�5?      �� *CanViewRegistrations+ �� 	Converter# [ .+�5@      �� $3��-You are not authorized to view registrations.��6   $��Center=�5A      $��Center=�5B      $��16(�5C      
���  ��5D      5E      5F      5G      