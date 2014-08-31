RedPlanetX
==========

A small tool to modify videos.

Here is the fourth concept version of RedPlanet. RedPlanetX (RedPlanet Xpress) is a tool which can modify video by addition of transparent layers onto the original video. For the moment it's still an experimental project, some functions don't work.

Written in C# and designed to be a tool for Windows (for the moment), it uses ffmpeg to extract images and encode them. A save in JSON is made at the opening of a new video and reloads the files for the next time. At this time, you cannot save or load a project. In the future, we could also use scripting in Python maybe.

If you want to download an alpha release, it's <a href="http://www.redarchive.hol.es/">available in an external site</a>. Otherwise, the whole project is in this site. So have fun with source.

For a world access, RedPlanetX is only in english for the moment !

---

There are graphical objects which can be added to the video area into layers :

<ul>
<li>Horizontal text (with normal text, text from subtitle with/without karaokes) - not complete</li>
<li>Vertical text (with normal text, text from subtitle with/without karaokes) - TODO</li>
<li>Text area (with normal text, text from subtitle with/without karaokes) - TODO</li>
<li>Shape (including a shape mode and a free design mode working like Points And Curves) - TODO</li>
<li>Drawing (paint mode) - TODO</li>
<li>Image - TODO</li>
<li>Video - TODO</li>
</ul>
