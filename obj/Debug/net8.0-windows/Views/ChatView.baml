   M S B A M L   `   `   `  ����I  EPRN212_Project, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:PRN212_Project.ViewsPRN212_Project.Views  5      E'clr-namespace:PRN212_Project.ViewModelsPRN212_Project.ViewModels  5      E'clr-namespace:PRN212_Project.ConvertersPRN212_Project.Converters  5      #    PRN212_Project.Views.ChatView   5      W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      4local"clr-namespace:PRN212_Project.Views     5      >
viewmodels'clr-namespace:PRN212_Project.ViewModels     5      >
converters'clr-namespace:PRN212_Project.Converters     5        � 	Resources  5      9   3PRN212_Project.Converters.MessageAlignmentConverter   MessageAlignmentConverter5   /PRN212_Project.Converters.MessageColorConverter  MessageColorConverter��%:   &	        &	        5   
   6/     5   
   6+   5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5   
   �� $��0��6   ���#3498db �� Padding$ 10q�63   $��0,50,0,0q�6@   ._�5      �� $��	Club Chat��6   
�������$��20(�6<   $
��Bold"�6J   5      �� 5!   
   -   5         ��Name$ ChatScrollViewer��5!      $��1��61   $
��Auto=�6>   $��	0,10,0,10q�6a   .��5"      �� ��6   �� *
Messages+��5#      �� 5$      .R�5%      �� $
��10,5q�6!   $ 10q�6/    �� CornerRadius$ 5��5'   !   $	��400��62   ��5&   !   �� *
SenderId+ �� 	Converter# [ ��5(   !   �� *
SenderId+# [  ._�5)      �� .+�5*   "   �� $
��Bold"�6M   $��0,0,0,5q�6_   ��6,   �� *Sender.FullName+6_   �� 5+   "   $
��Wrap=�6E   ��6,   �� *	Content+6E   �� 5,   "   $��10(�5-   ,   $��Right=�6:   $��0,5,0,0q�6V   ��5,   ,   �� *SentAt+ �� StringFormat$ HH:mm:ss, dd MMM yyyy��5-   V   5.      5/      50      52      53      � 56   
   $��2��6   $��10q�6    � ColumnDefinitions 57      �� 58      $��* �6#   �� 59      $
��Auto �6#   .+�5<      �� -   5        $��0��5<      $��10q�5=      $��14(�6#    �� AcceptsReturn$ False��61   ��5<   &   �� *MessageText+	 �� UpdateSourceTrigger$	 PropertyChanged=�5>      �� 5@      $��1��6   $
��Send��6%   $��10,0,0,0q�64   $
��15,5q�6F   ���#2ecc71
���������5A      �� *SendMessageCommand+6Q   5B      5C      5D      