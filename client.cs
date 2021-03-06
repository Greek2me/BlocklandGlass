//================================================================
//=	Title: 	Blockland Glass (i3)								                 =
//=	Author:	Jincux (9789)								                     		 =
//=	If you're looking at this, go you. either that, or you're a  =
//=	little skiddy trying to 'troll'						              		 =
//================================================================


//Object-based structure, for data's sake
function BLG::init() {
	new ScriptObject(BLG) {
		version = "1.0.0-beta.1";
		//address = "192.168.1.2";
		//netAddress = "192.168.1.2";
		address = "api.blocklandglass.com";
		netAddress = "blocklandglass.com";

		enableCLI = true;
	};

	//GlobalActionMap.bind("keyboard", ,"RTB_toggleOverlay");
}

function BLG::exec() {
	BLG::init();
	echo(" === Blockland Glass v" @ BLG.version @ " suiting up. ===");
	exec("./support/Support_TCPClient.cs");
	exec("./support/Support_Markdown.cs");

	echo(" ===                 Loading Interface                  ===");
	exec("./client/gui/profiles.cs");
	exec("./client/gui/GlassUpdatesGui.gui");
	exec("./BLG_VerifyAccount.gui"); //need to rename/move
	exec("./client/gui/GlassModManagerGui.gui");
	exec("./client/gui/GlassModManagerImage.gui");

	echo(" ===              Executing Important Stuff             ===");
	exec("./common/GlassFileData.cs");
	exec("./common/GlassDownloadManager.cs");
	exec("./common/GlassRTBSupport.cs");
	exec("./common/GlassUpdaterSupport.cs");
	exec("./client/GlassAuth.cs");
	exec("./client/GlassModManager.cs");

	echo(" ===                   Starting it up                   ===");
	GlassAuth::init();
	GlassDownloadManager::init();
	GlassRTBSupport::init();
	GlassUpdaterSupport::verifyInstall();

	GlassModManager::init();

	echo(" ===            Drunkenly staggering forward            ===");

	exec("config/BLG/client/mm.cs");

	//tests
	if($BLG::MM::Colorset $= "") {
		$BLG::MM::Colorset = "Add-Ons/System_BlocklandGlass/colorset_default.txt";
	}

	if($BLG::MM::Keybind $= "") {
		$BLG::MM::Keybind = "keyboard\tctrl m";
	}

  GlassModManagerGui_Prefs_Keybind.setText("\c4" @ strupr(getField($BLG::MM::Keybind, 1)));

	%bind = $BLG::MM::Keybind;
	GlobalActionMap.bind(getField(%bind, 0), getField(%bind, 1), "GlassModManager_keybind");
}

function Glass::doWelcomeMessage() {
	if(!$BLG::MM::WelcomeMessage) messageBoxOk("Welcome to Blockland Glass", "<font:arial bold:20>Welcome to Blockland Glass<font:arial: 14><br><br>Thank you very much for downloading Blockland Glass!<br><br>To get started, press <font:arial bold:14>CTRL M<font:arial:14>!", "Glass::welcomeMessageSeen();");
}

function Glass::welcomeMessageSeen() {
	$BLG::MM::WelcomeMessage = true;
	export("$BLG::MM::*", "config/BLG/client/mm.cs");
}

BLG::exec();

package GlassPrefs {
	function onExit() {
		export("$BLG::MM::*", "config/BLG/client/mm.cs");
		parent::onExit();
	}

	function MM_AuthBar::blinkSuccess(%this) {
		Glass::doWelcomeMessage();
		parent::blinkSuccess(%this);
	}
};
activatePackage(GlassPrefs);
