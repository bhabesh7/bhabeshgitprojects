   M S B A M L   `   `   `  ����  ColorToGrayScale!    ColorToGrayScale.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  
MainWindow��5   	   $	��350��6   $	��525��6)   .��5      �  � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5	      $��* �6    � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   .+�5   
   �� $��0��6   $��0��6"   .+�5      �� ��6   �� *OriginalPath+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=��� 5      $��
Load Image��6:   $	��100��6O   ��6   �� *LoadImageCommand+6O   5      �� 5   
   $��0��6   $��1��6"   .+�5      �� ��6   �� *GrayScaleImageSource+$ TwoWay=�$ PropertyChanged=��� 5      $��Make it Gray��6:   $	��100��6Q   ��6   �� *GrayImageCommand+6Q   5      5      5      