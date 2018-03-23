<%@ Control Language="c#" AutoEventWireup="false" Codebehind="menuManut.ascx.cs" Inherits="SistMars.include.menuManut" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"%>
<style type="text/css">
.clTopMenu       {position:absolute; width:110px; height:150px; clip:rect(0px 110px 14px 0px); layer-background-color:#eeeeee; background-color:#eeeeee; z-index:31; visibility:hidden;}
.clTopMenuBottom {position:absolute; width:110px; height:3px; clip:rect(0px 110px 3px 0px); top:11; layer-background-color:#cecfce; background-color:#cecfce; z-index:2;}
.clTopMenuText   {position:absolute; width:100px; left:5px; top:15px; font-family:arial,helvetica,sans-serif; font-size:9px; background-color:#eeeeee; z-index:1;} 
</style>
<script language="JavaScript" type="text/javascript">
/**********************************************************************************   
NewsMenu 
*   Copyright (C) 2001 Thomas Brattli
*   This script was released at DHTMLCentral.com
*   Visit for more great scripts!
*   This may be used and changed freely as long as this msg is intact!
*   We will also appreciate any links you could give us.
*
*   Made by Thomas Brattli
*
*   Script date: 09/23/2001 (keep this date to check versions) 
*********************************************************************************/
function lib_bwcheck(){ //Browsercheck (needed)
	this.ver=navigator.appVersion
	this.agent=navigator.userAgent
	this.dom=document.getElementById?1:0
	this.opera5=(navigator.userAgent.indexOf("Opera")>-1 && document.getElementById)?1:0
	this.ie5=(this.ver.indexOf("MSIE 5")>-1 && this.dom && !this.opera5)?1:0; 
	this.ie6=(this.ver.indexOf("MSIE 6")>-1 && this.dom && !this.opera5)?1:0;
	this.ie4=(document.all && !this.dom && !this.opera5)?1:0;
	this.ie=this.ie4||this.ie5||this.ie6
	this.mac=this.agent.indexOf("Mac")>-1
	this.ns6=(this.dom && parseInt(this.ver) >= 5) ?1:0; 
	this.ns4=(document.layers && !this.dom)?1:0;
	this.bw=(this.ie6 || this.ie5 || this.ie4 || this.ns4 || this.ns6 || this.opera5)
	return this
}
var bw=lib_bwcheck()
/********************************************************************************
If you want to change the appearance of the text, background-colors, size or
anything do that in the style tag above.

This menu might not be as easy to adapt to your own site, but please
play around with it before you mail me for help...
****************************************************************************/

/***************************************************************************
Variables to set.
****************************************************************************/

//There are 2 ways these menus can be placed
// 0 = column
// 1 = row
nPlace=0


//How many menus do you have? (remember to copy and add divs in the body if you add menus)
var nNumberOfMenus=1

var nMwidth=110 //The width on the menus (set the width in the stylesheet as well)
var nPxbetween=20 //Pixels between the menus
var nFromleft=10 //The first menus left position
var nFromtop=5 //The top position of the menus
var nBgcolor='#CECFCE' //The bgColor of the bottom mouseover div 
var nBgcolorchangeto='#6380BC' //The bgColor to change to
var nImageheight=11 //The position the mouseover line div will stop at when going up!

/***************************************************************************
You shouldn't have to change anything below this
****************************************************************************/
//Object constructor
function makeNewsMenu(obj,nest){
	nest=(!nest) ? "":'document.'+nest+'.'					
   	this.css=bw.dom? document.getElementById(obj).style:bw.ie4?document.all[obj].style:bw.ns4?eval(nest+"document.layers." +obj):0;		
	this.evnt=bw.dom? document.getElementById(obj):bw.ie4?document.all[obj]:bw.ns4?eval(nest+"document.layers." +obj):0;			
	this.scrollHeight=bw.ns4?this.css.document.height:this.evnt.offsetHeight
	this.moveIt=b_moveIt;this.bgChange=b_bgChange;
	this.slideUp=b_slideUp; this.slideDown=b_slideDown;
	this.clipTo=b_clipTo;
    this.obj = obj + "Object"; 	eval(this.obj + "=this")		
}
//Objects methods

// A unit of measure that will be added when setting the position of a layer.
var px = bw.ns4||window.opera?"":"px";

function b_moveIt(x,y){this.x=x; this.y=y; this.css.left=this.x+px; this.css.top=this.y+px;}
function b_bgChange(color){this.css.backgroundColor=color; this.css.bgColor=color; this.css.background=color;}
function b_clipTo(t,r,b,l){
	if(bw.ns4){this.css.clip.top=t; this.css.clip.right=r; this.css.clip.bottom=b; this.css.clip.left=l
	}else this.css.clip="rect("+t+"px "+r+"px "+b+"px "+l+"px)";
}
function b_slideUp(ystop,moveby,speed,fn,wh){
	if(!this.slideactive){
		if(this.y>ystop){
			this.moveIt(this.x,this.y-5); eval(wh)
			setTimeout(this.obj+".slideUp("+ystop+","+moveby+","+speed+",'"+fn+"','"+wh+"')",speed)
		}else{
			this.slideactive=false; this.moveIt(0,ystop); eval(fn)
		}
	}
}
function b_slideDown(ystop,moveby,speed,fn,wh){
	if(!this.slideactive){
		if(this.y<ystop){
			this.moveIt(this.x,this.y+5); eval(wh)
			setTimeout(this.obj+".slideDown("+ystop+","+moveby+","+speed+",'"+fn+"','"+wh+"')",speed)
		}else{
			this.slideactive=false; this.moveIt(0,ystop); eval(fn)
		}
	}
}
//Initiating the page, making cross-browser objects
function newsMenuInit(){
	oTopMenu=new Array()
	zindex=10
	for(i=0;i<=nNumberOfMenus;i++){
		oTopMenu[i]=new Array()
		oTopMenu[i][0]=new makeNewsMenu('divTopMenu'+i)
		oTopMenu[i][1]=new makeNewsMenu('divTopMenuBottom'+i,'divTopMenu'+i)
		oTopMenu[i][2]=new makeNewsMenu('divTopMenuText'+i,'divTopMenu'+i)
		oTopMenu[i][1].moveIt(0,nImageheight)
		oTopMenu[i][0].clipTo(0,nMwidth,nImageheight+3,0)
		if(!nPlace) oTopMenu[i][0].moveIt(i*nMwidth+nFromleft+(i*nPxbetween),nFromtop)
		else{
			oTopMenu[i][0].moveIt(nFromleft,i*nImageheight+nFromtop+(i*nPxbetween))
			oTopMenu[i][0].css.zIndex=zindex--
		}
		oTopMenu[i][0].css.visibility="visible"
	}
}
//Moves the menu
function topMenu(num){
	if(oTopMenu[num][1].y==nImageheight) oTopMenu[num][1].slideDown(oTopMenu[num][2].scrollHeight+20,10,40,'oTopMenu['+num+'][0].clipTo(0,nMwidth,oTopMenu['+num+'][1].y+3,0)','oTopMenu['+num+'][0].clipTo(0,nMwidth,oTopMenu['+num+'][1].y+3,0)')
	else if(oTopMenu[num][1].y==oTopMenu[num][2].scrollHeight+20) oTopMenu[num][1].slideUp(nImageheight,10,40,'oTopMenu['+num+'][0].clipTo(0,nMwidth,oTopMenu['+num+'][1].y+3,0)','oTopMenu['+num+'][0].clipTo(0,nMwidth,oTopMenu['+num+'][1].y+3,0)')
}
//Changes background onmouseover
function menuOver(num){oTopMenu[num][1].bgChange(nBgcolorchangeto)}
function menuOut(num){oTopMenu[num][1].bgChange(nBgcolor)}

//Calls the init function onload if the browser is ok...
if (bw.bw) onload = newsMenuInit;

/***************
Multiple Scripts
If you have two or more scripts that use the onload event, probably only one will run (the last one).
Here is a solution for starting multiple scripts onload:
   1. Delete or comment out all the onload assignments, onload=initScroll and things like that.
   2. Put the onload assignments in the body tag like in this example, note that they must have braces ().
   Example: <body onload="initScroll(); initTooltips(); initMenu();">
**************/
</script>
<!-- Remember the "news" "key control" and "page contols" text are images, you probably want to change those
with your own images. If your own images have different sizes please adjust the height and clips
of the divs, and change the nImageheight variable in the script. Good luck -->
<div id="divTopMenu0" class="clTopMenu"><a href="#" onmouseover="menuOver(0)" onmouseout="menuOut(0)" onclick="topMenu(0); return false;" onfocus="if(this.blur)this.blur();"><img src="../images/menuManut.gif" width="110" height="11" alt="" border="0" align="top"></a>
	<div id="divTopMenuText0" class="clTopMenuText">
		<a href="../manut/texto.aspx">Textos</a><br>
		<a href="../manut/caracteristica.aspx">Características</a><br>
		<a href="../manut/pontuacao.aspx">Pontuação</a><br>
		<a href="../manut/autoconhecimento.aspx">Auto-conhecimento</a>
	</div>
	<div id="divTopMenuBottom0" class="clTopMenuBottom"></div>
</div>
<div id="divTopMenu1" class="clTopMenu"><a href="#" onmouseover="menuOver(1)" onmouseout="menuOut(1)" onclick="topMenu(1); return false;" onfocus="if(this.blur)this.blur();"><img src="../images/menuRelat.gif" width="110" height="11" alt="" border="0" align="top"></a>
	<div id="divTopMenuText1" class="clTopMenuText">
		<a href="../relatorio/enquetes.aspx">Enquetes</a><br>
		<a href="../relatorio/analises.aspx">Análises</a>
	</div>
	<div id="divTopMenuBottom1" class="clTopMenuBottom"></div>
</div>
<!-- To add a new menu just copy these lines:
<div id="divTopMenuN" class="clTopMenu"><a href="#" onmouseover="menuOver(N)" onmouseout="menuOut(N)" onclick="topMenu(N); return false;" onfocus="if(this.blur)this.blur();">HEADING IMAGE GOES HERE</a>
	<div id="divTopMenuTextN" class="clTopMenuText">
		TEXT HERE	
	</div>
	<div id="divTopMenuBottomN" class="clTopMenuBottom"></div>
</div>
And change the letter N to a number higher then the last menu...
(and remember to set the variable nNumberOfMenus in the script to 
the same number of menus you have (remember it starts counting
at 0) --><br>
