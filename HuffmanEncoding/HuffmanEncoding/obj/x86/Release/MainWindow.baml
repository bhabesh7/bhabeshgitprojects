   M S B A M L   `   `   `  ���� J  FHuffmanEncoding, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:HuffmanEncodingHuffmanEncoding  M ISystem, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089/$clr-namespace:System;assembly=SystemSystem      HuffmanEncoding.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     /localclr-namespace:HuffmanEncoding     5objx$clr-namespace:System;assembly=System       � Title$  
MainWindow��$	��350��$	��525�� � 	Resources .   (HuffmanEncoding.TextToCharCountConverter   textToCharCountConverter-   'HuffmanEncoding.TextToBitCountConverter  textToBitCountConverter4   .HuffmanEncoding.PercentageCalculationConverter " percentageCalculationConverter �� 
TargetType �� Property �� Value��%Z   &	        &	      &	 
     '��            �� " ��.���  � ��$ 16(�.���  � RowDefinitions �� �� ��  � ColumnDefinitions �� $	��150 ��� $��* �.+��� -   $��btnCompress��$��0��$��0��$��Compress��$��50��$	��100���� -   $��textBox1��$��0��$��1��$	��100��$	��350��$
��Left=�$��Center=� �� TextWrapping$
 Wrap=�
��������� -   $��listViewEncoding��$��1��$��1��$��5q����� ����  �� DataType!   HuffmanEncoding.DisplayData"  .R��� 	 �� Orientation$	 
Horizontal=�.+��� ���� *Key+�� $��=���� ���� *Value+�� -   $��txbHuffmannCode��$��1��$��1��$
��Wrap=�$��5q����� *Key+�� -   $��StatsSP��$��2��$��0��$	 Vertical=�$��2��$��10q�.+��� $	 
Horizontal=�.+��� $��Input Word Count: ���� -   $��txbCharCount������ 
 �� ElementName$
 textBox1�� �� Path$
 Text� �� 	Converter# [  �� $	 
Horizontal=�.+��� $��Bit Count: ���� -   $��txbBitCount������ $
 txbCharCount��$
 Text�# [ �� $	 
Horizontal=�.+��� $��Huffman Bit Count: ���� -   $��txbHuffmannBitCount������ $
 txbHuffmannCode��$
 Text�# [  �� $	 
Horizontal=�.+��� $��% Compression: ���� ��`�  `� 	Converter# [  `� UpdateSourceTrigger$ Default=�.1��� $
 txbBitCount��$
 Text��� $
 txbHuffmannBitCount��$
 Text�