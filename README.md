RedPlanetX
==========

A small tool to modify videos.

Here is the third concept version of RedPlanet. RedPlanetX (RedPlanet Xpress) is a tool which can modify video by addition of transparent layers onto the original video. For the moment it's still an experimental project, some functions don't work.

Written in Java, it uses Xuggle to extract images of a video (the first time you open it, there is a save in a JSON file for the next times) and the encoding of your test project. In the future, we could also use scripting in JRuby, Jython and Rhino (maybe). For the moment you can test it with care, by writing by example a simple hello world. There isn't a real integration of scripting for the moment.

The project ins't ready for a download (which will be <a href="http://www.redarchive.hol.es/">available in an external site</a>), if you would to see it, you must compile it by using NetBeans and the required libraries which are <a href="https://github.com/artclarke/xuggle-xuggler">Xuggle</a> and its dependencies, <a href="http://code.google.com/p/json-simple/">JSON simple</a>, <a href="https://github.com/jruby/jruby">JRuby</a> and <a href="http://www.jython.org/">Jython</a>.

For a world access, RedPlanetX is only in english for the moment (I develop it in english but if you see errors it's my fault, I cannot do some miracles because I am french !). I hope you can understand my english XD !

---

There are graphical objects which can be added to the video area into layers :

<ul>
<li>Horizontal text (with normal text, text from subtitle with/without karaokes) - not complete</li>
<li>Vertical text (with normal text, text from subtitle with/without karaokes) - not complete</li>
<li>Text area (with normal text, text from subtitle with/without karaokes) - TODO</li>
<li>Rectangle (and square) - TODO</li>
<li>Round rectangle (and round square) - TODO</li>
<li>Ellipse (and circle) - TODO</li>
<li>Shape (working like Points And Curves) - TODO</li>
<li>Drawing (paint mode) - TODO</li>
<li>Image - TODO</li>
<li>Video - TODO</li>
</ul>
