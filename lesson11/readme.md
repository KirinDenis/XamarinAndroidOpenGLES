﻿После того как «овладели общим видом» OpenGL ES, появилось еще больше вопросов и стало понятно что просто умение использовать OpenGL это еще не ВСЯ 3D графика. 
Для более глубоко погружения в вопрос, приняли решение написать собственную реализацию 3D рендеринга, используя только Canvas (растр) и CPU. 

Возвращаемся к Android примеру использования Canvas (Lesson 1).

Начинаем с 2D трансформаций:

1) Рассматриваем формулу радиус вектора еще раз. Реализуем вращение (трансформацию) плоского треугольника используя радиус вектор.

2) Рассматриваем формулу поворота вектора, в ней не используется радиус. Реализуем вращение треугольника используя формулу поворота. Именно эту формулу использует OpenGL при вращательной трансформации. (Показываем как вывести эту формулу из формулы радиус вектора ЭТО ВАЖНО). 

3) Добавляем остальные трансформации — перемещение и скалинг (растягивание).

4) Переносим полученную формулу в матричное представление, поясняем правила умножения матрицы на вектор и демонстрируем как OpenGL реализовал матрицу трансформации, и как альтернативу приводим способ трансформации без использования матриц.

Полезные шпаргалки:
https://docs.google.com/document/d/1LVbNnjfq0ON7eN2l8yvKyEEtGIuzzx53iMB9KYTP-5Q/edit

Video:

https://youtu.be/ktY2h2R2dxQ

https://youtu.be/aFAXJ0zOows

https://youtu.be/slW1rl-U-YI
