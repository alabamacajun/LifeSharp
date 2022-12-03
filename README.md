# LifeSharp
## Conway's Game of Life in living color.
C# project running in XAML built in Net5 and moved to Net7. 
  Just change thet net version to 6 or 5 in the project if you don't want to install recent versions.
  
Current implementation creates a glider when you start it then runs at several clicks per second.
Optimizations are minimal so I'm sure there is room for that ;) Hint in the LifeCell class Cells is newed up each cycle.

Standard cell rules 2/3.
Color
  1) Green - newborn
  2) Yellow - normal cell
  3) Red - end of life
