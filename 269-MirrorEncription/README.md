<h1 align="center"> Challenge #269 [Intermediate] Mirror Encryption </h1>

This challenge consists of creating a program that encrypts and decrypts a
word using a mirror field. Inside this mirror field mirrors are represented
with '\\' and '/'. Here you can see a sample grid with just 8 letters and 2 
mirrors:
<pre>
 ab
A \c
B\ d
 CD
</pre>
   
In this case the input word 'Baba' will be translated to 'Cdcd'.<br>

<h2> The program </h2>

The grid of the program consists of all the letters of the english vocabulary,
and both the mirrors and the word to encrypt/decrypt are inputed as a text file. 
Here you have an example of my program's output using the file
'alibaba.txt' (available in the repository) as an input:

<pre>
Reading grid...

   | a | b | c | d | e | f | g | h | i | j | k | l | m |  
------------------------------------------------------------
 A |   | \ |   |   | \ | \ |   |   | / | \ |   |   |   | n
------------------------------------------------------------
 B |   |   |   |   |   |   |   |   | \ | / | / |   | \ | o
------------------------------------------------------------
 C | / |   |   | / |   |   |   |   |   | / |   |   | / | p
------------------------------------------------------------
 D | / |   |   |   |   |   | \ |   |   |   |   |   | \ | q
------------------------------------------------------------
 E |   | / |   |   | \ |   |   |   |   |   |   |   |   | r
------------------------------------------------------------
 F |   |   | / |   |   |   |   |   |   | / |   |   |   | s
------------------------------------------------------------
 G | \ |   |   | / |   |   |   |   |   |   | \ |   |   | t
------------------------------------------------------------
 H |   |   |   |   |   | \ |   |   |   |   |   |   |   | u
------------------------------------------------------------
 I | \ | / |   |   |   |   |   |   |   |   |   |   |   | v
------------------------------------------------------------
 J | / |   |   |   | \ |   |   |   | / |   | \ |   |   | w
------------------------------------------------------------
 K |   |   |   |   |   |   |   |   |   |   | \ |   |   | x
------------------------------------------------------------
 L |   |   |   |   | \ | / |   |   |   |   | / |   |   | y
------------------------------------------------------------
 M |   |   |   | / |   |   |   |   | / |   | / |   | / | z
------------------------------------------------------------
   | N | O | P | Q | R | S | T | U | V | W | X | Y | Z |  

Input word: Alibaba.
Result: EYfrCrC.</pre><br>

If you want to know more information about the challenge you can check the [reddit post](https://www.reddit.com/r/dailyprogrammer/comments/4m3ddb/20160601_challenge_269_intermediate_mirror/).
