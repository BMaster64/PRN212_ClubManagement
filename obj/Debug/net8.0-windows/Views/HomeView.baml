   M S B A M L   `   `   `  ����I  EPRN212_Project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:PRN212_Project.ViewsPRN212_Project.Views  5   	   #    PRN212_Project.Views.HomeView   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:PRN212_Project.Views     5   	     � Title$  HomeView��5   	   $	��600��6   $	��900��6'    � WindowStartupLocation$ CenterScreen=�5	   	    � WindowStyle$
 None=�5
   	    � 
ResizeMode$ NoResize=�5   	   .��5      � .+�5   
   �� -   5        ���#2C3E50$��50��5   &   $	��Top=�62   ._�5      � .+�5      �� $��20(�5      $
��Bold"�5      
�������$��Center=�5      $��	205,0,0,0q�5      $
��Left=�5      $	��620��5      ��5      �� *
ClubName+5      �� 5       �� Orientation$ 
Horizontal=�6   $��Right=�66   .+�5      �� -   5        $��_��5      $��30��6)   $��30��64   $��Center=�6@   $��0,0,5,0q�6[   
������ 
�������
������ $��20(�5   �   �� 5      -   5        $��X��5      $��30��6)   $��30��64   $��Center=�6@   $��0,0,10,0q�6[   
������ 
�������
������ $��20(�5   }   5      5      5       ]� 5#   
   $	��200��6   ���#2C3E50$��	0,0,700,0q�65   .+�5$      �� $��10q�6   $	��582��6%   .+�5%      �� $��
Navigation��6   $��28(�6.   $
��Bold"�6<   
�������$��10q�6a   �� 5'      $��Notification��6      NavButtonStyle#��[  60   $��Notification��6|   ��6X   �� *NavigateCommand+6|   �� 5(      $��Member��6   #��[  6*   $��Member��6v   ��6R   �� *NavigateCommand+6v   �� 5)      $��Event��6   #��[  6)   $��Event��6u   ��6Q   �� *NavigateCommand+6u   �� 5*      $
��Chat��6   #��[  6(   $
��Chat��6t   ��6P   �� *NavigateCommand+y�6�   �� *ChatTabVisibility+�� 5+      $��Report��6   #��[  6*   $��Report��6v   ��6R   �� *NavigateCommand+y�6�   �� *ReportTabVisibility+�� 5-      y�6   �� *AdminVisibility+.+�5.      �� $��Club Management��6   #��[  5/      $��ClubManagement��51      ��50      �� *NavigateCommand+51      �� 52      $��Club Requests��6   #��[  53      $��ClubRequests��55      ��54      �� *NavigateCommand+55      56      �� 57      -   5        $��Log Out��57      #��[  6+   $��20, 250, 20, 0q�6n   
���  ��$
��Bold"�6�   $��20(�6�   58      59      �� 5<   
   $��	200,0,0,0q�69   ��6   �� *CurrentView+69   5=      5>      