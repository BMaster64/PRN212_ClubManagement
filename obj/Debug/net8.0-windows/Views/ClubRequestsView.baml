   M S B A M L   `   `   `  ����I  EPRN212_Project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:PRN212_Project.ViewsPRN212_Project.Views  5      E'clr-namespace:PRN212_Project.ConvertersPRN212_Project.Converters  5      +    %PRN212_Project.Views.ClubRequestsView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:PRN212_Project.Views     5      >
converters'clr-namespace:PRN212_Project.Converters     5        � 	Resources  5
      7   1PRN212_Project.Converters.UserTypeToRoleConverter &  "RequestStatusToVisibilityConverter��%   &	          5   
   6-   5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $ ��Club Registration Requests��6   $��24(�5      $
��Bold"�5      $��
10,50,0,10q�5      &   System.Windows.Controls.DataGrid  5   
   $��1��6      AutoGenerateColumns$ False��5         
IsReadOnly$
 True��5      $��10q�5      ��5      �� *ClubRequests+   Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5       ,  &System.Windows.Controls.DataGridColumn   Header$ 	Club Name��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$ * 6U   1  +System.Windows.Controls.DataGridBoundColumn   Binding 68   �� *
ClubName+6U     5!      $ President Name��6%   $ * 6c    6=   �� *PresidentFullName+6c     5"      $ President ID��6%   $ * 6b    6;   �� *PresidentStudentId+6b     5#      $ Requested At��6%   $ * 6�    6;   �� *RequestedAt+ �� StringFormat$ {0:MM/dd/yyyy HH:mm}��6�   4  .System.Windows.Controls.DataGridTemplateColumn  5$      $ Actions��6)   $	 175 6:   	   CellTemplate	 5%      �� 5&      .R�5'      �� 
 �� Orientation$
 
Horizontal=�6)   y�5(   )   �� *Status+ �� 	Converter# [  .+�5)   "   �� $��Approve��6)     NavButtonStyle#��[ 5-   )   
��� d �$��14,0,5,0q�5/   )   $��70��6;   ��5*   )   �� *#!DataContext.ApproveRequestCommand+ �� RelativeSource ��  �� AncestorType DataGrid��5,   )   �� 5/   ;   �� 51   "   $��Decline��6)   #��[ 55   )   
���\\��$��0,0,5,0q�57   )   $��70��6:   $��30��6E   ��52   )   �� *#!DataContext.DeclineRequestCommand+ ��  DataGrid��54   )   �� 57   E   58      59      5;      5=      5>      5?      