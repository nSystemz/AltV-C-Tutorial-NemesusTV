import * as alt from 'alt';
import * as game from 'natives';
import * as NativeUI from './includes/NativeUI/NativeUI';

const Menu = NativeUI.Menu;
const UIMenuItem = NativeUI.UIMenuItem;
const UIMenuListItem = NativeUI.UIMenuListItem;
const UIMenuCheckboxItem = NativeUI.UIMenuCheckboxItem;
const UIMenuSliderItem = NativeUI.UIMenuSliderItem;
const BadgeStyle = NativeUI.BadgeStyle;
const Point = NativeUI.Point;
const ItemsCollection = NativeUI.ItemsCollection;
const Color = NativeUI.Color;
const ListItem = NativeUI.ListItem;
const maxListItems = 800;

let player = alt.Player.local;

const menu = new Menu("Clothes Tool", "", new Point(50, 50));
let menumask = new UIMenuItem("Mask", "");
let menuhairs = new UIMenuItem("Hair", "");
let menutorso = new UIMenuItem("Torso", "");
let menulegs = new UIMenuItem("Legs", "");
let menubags = new UIMenuItem("Bags and parachute", "");
let menushoes = new UIMenuItem("Shoes", "");
let menuacc = new UIMenuItem("Accessories", "");
let menuunder = new UIMenuItem("Undershirt", "");
let menuarmor = new UIMenuItem("Armor", "")
let menudecals = new UIMenuItem("Decals", "");
let menutops = new UIMenuItem("Tops", "");
let menuhat = new UIMenuItem("Hats", "");
let menuglasses = new UIMenuItem("Glasses", "");
let menuears = new UIMenuItem("Ears", "");
let menuwatches = new UIMenuItem("Watches", "");
let menubracelets = new UIMenuItem("Bracelets", "");


///////////////////////MASK////////////////////////////////////////
let maskDrawable = [];

for (let i = 0; i < game.getNumberOfPedDrawableVariations(player.scriptID, 1) + 1; i++) {
	maskDrawable.push(i.toString());
}

let maskTextureArray = [];
let maskTextureLimit = game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1));
for (let i = 0; i < maskTextureLimit + 1; i++) {
	maskTextureArray.push(i.toString());
}

const MaskMenu = new Menu("Mask Menu", "", new Point(50, 50));
MaskMenu.Visible = false;
MaskMenu.GetTitle().Scale = 0.9;
menu.AddSubMenu(MaskMenu, menumask);

let maskItem = new NativeUI.UIMenuAutoListItem("Mask", "", -maxListItems, maxListItems, 1, game.getNumberOfPedDrawableVariations(player.scriptID, 1));
let maskTextureItem = new NativeUI.UIMenuAutoListItem("Color", "", -maxListItems, maxListItems, 0, game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)));


MaskMenu.AddItem(maskItem);
MaskMenu.AddItem(maskTextureItem);
MaskMenu.ItemSelect.on((selectedItem, selectedItemIndex) => {
	let drawable = maskItem.SelectedValue;
	let texture = maskTextureItem.SelectedValue;

	alt.log(selectedItemIndex);
	switch (selectedItemIndex) {
		case 0: {
			alt.log(drawable);
			game.setPedComponentVariation(player.scriptID, 1, drawable, 0, 0);

			let maskTextureNewArray = [];
			for (let i = 0; i < game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)) + 1; i++) {
				maskTextureNewArray.push(i.toString());
			}
			
			maskTextureItem.Collection = new ItemsCollection(maskTextureNewArray).getListItems();
			maskTextureItem.Index = 0;
			break;
		}
		
		case 1: {
			game.setPedComponentVariation(player.scriptID, 1, drawable, texture, 0);
			break;
		}

		default: {
		}
	}
});

/////////////////////// END MASK////////////////////////////////////////

///////////////////////HAIRS////////////////////////////////////////
let hairsDrawable = [];

for (let i = 0; i < game.getNumberOfPedDrawableVariations(player.scriptID, 1) + 1; i++) {
	hairsDrawable.push(i.toString());
}

let hairsTextureArray = [];
let hairsTextureLimit = game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1));
for (let i = 0; i < hairsTextureLimit + 1; i++) {
	hairsTextureArray.push(i.toString());
}

const hairsMenu = new Menu("Hairs menu", "", new Point(50, 50));
hairsMenu.Visible = false;
hairsMenu.GetTitle().Scale = 0.9;
menu.AddSubMenu(hairsMenu, menuhairs);

let hairsItem = new NativeUI.UIMenuAutoListItem("Hairs", ".", -maxListItems, maxListItems, 1, game.getNumberOfPedDrawableVariations(player.scriptID, 1));
let hairsTextureItem = new NativeUI.UIMenuAutoListItem("Color", "", -maxListItems, maxListItems, 0, game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)));

hairsMenu.AddItem(hairsItem);
hairsMenu.AddItem(hairsTextureItem);
hairsMenu.ItemSelect.on((selectedItem, selectedItemIndex) => {
	let drawable = hairsItem.SelectedValue;
	let texture = hairsTextureItem.SelectedValue;

	alt.log(selectedItemIndex);
	switch (selectedItemIndex) {
		case 0: {
			alt.log(drawable);
			game.setPedComponentVariation(player.scriptID, 2, drawable, 0, 0);

			let hairsTextureNewArray = [];
			for (let i = 0; i < game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)) + 1; i++) {
				hairsTextureNewArray.push(i.toString());
			}
			
			hairsTextureItem.Collection = new ItemsCollection(hairsTextureNewArray).getListItems();
			hairsTextureItem.Index = 0;
			break;
		}
		case 1: {
			game.setPedHeadBlendData(player.scriptID, 0, 0, 0, 0, 0, 0, 0, 0, 0, false);
			game.setPedHairColor(player.scriptID, texture, texture);
			break;
		}

		default: {
		}
	}
});

/////////////////////// END HAIRS////////////////////////////////////////

///////////////////////TORSO////////////////////////////////////////
let TorsoDrawable = [];

for (let i = 0; i < game.getNumberOfPedDrawableVariations(player.scriptID, 1) + 1; i++) {
	TorsoDrawable.push(i.toString());
}

let TorsoTextureArray = [];
let TorsoTextureLimit = game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1));
for (let i = 0; i < TorsoTextureLimit + 1; i++) {
	TorsoTextureArray.push(i.toString());
}

const TorsoMenu = new Menu("Torso Menu", "", new Point(50, 50));
TorsoMenu.Visible = false;
TorsoMenu.GetTitle().Scale = 0.9;
menu.AddSubMenu(TorsoMenu, menutorso);

let TorsoItem = new NativeUI.UIMenuAutoListItem("Torso", "", -maxListItems, maxListItems, 1, game.getNumberOfPedDrawableVariations(player.scriptID, 1));
let TorsoTextureItem = new NativeUI.UIMenuAutoListItem("Color", "", -maxListItems, maxListItems, 0, game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)));

TorsoMenu.AddItem(TorsoItem);
TorsoMenu.AddItem(TorsoTextureItem);
TorsoMenu.ItemSelect.on((selectedItem, selectedItemIndex) => {
	let drawable = TorsoItem.SelectedValue;
	let texture = TorsoTextureItem.SelectedValue;

	switch (selectedItemIndex) {
		case 0: {
			game.setPedComponentVariation(player.scriptID, 3, drawable, 0, 0);

			let TorsoTextureNewArray = [];
			for (let i = 0; i < game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)) + 1; i++) {
				TorsoTextureNewArray.push(i.toString());
			}
			
			TorsoTextureItem.Collection = new ItemsCollection(TorsoTextureNewArray).getListItems();
			TorsoTextureItem.Index = 0;
			break;
		}
		
		case 1: {
			game.setPedComponentVariation(player.scriptID, 3, drawable, texture, 0);
			break;
		}

		default: {
		}
	}
});

/////////////////////// END TORSO////////////////////////////////////////


///////////////////////LEGS////////////////////////////////////////
let LegsDrawable = [];

for (let i = 0; i < game.getNumberOfPedDrawableVariations(player.scriptID, 1) + 1; i++) {
	LegsDrawable.push(i.toString());
}

let LegsTextureArray = [];
let LegsTextureLimit = game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1));
for (let i = 0; i < LegsTextureLimit + 1; i++) {
	LegsTextureArray.push(i.toString());
}

const LegsMenu = new Menu("Legs Menu", "", new Point(50, 50));
LegsMenu.Visible = false;
LegsMenu.GetTitle().Scale = 0.9;
menu.AddSubMenu(LegsMenu, menulegs);

let LegsItem = new NativeUI.UIMenuAutoListItem("Pants", "", -maxListItems, maxListItems, 1, game.getNumberOfPedDrawableVariations(player.scriptID, 1));
let LegsTextureItem = new NativeUI.UIMenuAutoListItem("Color", "", -maxListItems, maxListItems, 0, game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)));

LegsMenu.AddItem(LegsItem);
LegsMenu.AddItem(LegsTextureItem);
LegsMenu.ItemSelect.on((selectedItem, selectedItemIndex) => {
	let drawable = LegsItem.SelectedValue;
	let texture = LegsTextureItem.SelectedValue;

	switch (selectedItemIndex) {
		case 0: {
			game.setPedComponentVariation(player.scriptID, 4, drawable, 0, 0);

			let LegsTextureNewArray = [];
			for (let i = 0; i < game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)) + 1; i++) {
				LegsTextureNewArray.push(i.toString());
			}
			
			LegsTextureItem.Collection = new ItemsCollection(LegsTextureNewArray).getListItems();
			LegsTextureItem.Index = 0;
			break;
		}
		
		case 1: {
			game.setPedComponentVariation(player.scriptID, 4, drawable, texture, 0);
			break;
		}

		default: {
		}
	}
});

///////////////////////LEGS////////////////////////////////////////




///////////////////////BAGS////////////////////////////////////////
let BagsDrawable = [];

for (let i = 0; i < game.getNumberOfPedDrawableVariations(player.scriptID, 1) + 1; i++) {
	BagsDrawable.push(i.toString());
}

let BagsTextureArray = [];
let BagsTextureLimit = game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1));
for (let i = 0; i < BagsTextureLimit + 1; i++) {
	BagsTextureArray.push(i.toString());
}

const BagsMenu = new Menu("Bags menu", "", new Point(50, 50));
BagsMenu.Visible = false;
BagsMenu.GetTitle().Scale = 0.9;
menu.AddSubMenu(BagsMenu, menubags);

let BagsItem = new NativeUI.UIMenuAutoListItem("Bags", "", -maxListItems, maxListItems, 1, game.getNumberOfPedDrawableVariations(player.scriptID, 1));
let BagsTextureItem = new NativeUI.UIMenuAutoListItem("Color", "", -maxListItems, maxListItems, 0, game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)));

BagsMenu.AddItem(BagsItem);
BagsMenu.AddItem(BagsTextureItem);
BagsMenu.ItemSelect.on((selectedItem, selectedItemIndex) => {
	let drawable = BagsItem.SelectedValue;
	let texture = BagsTextureItem.SelectedValue;

	switch (selectedItemIndex) {
		case 0: {
			game.setPedComponentVariation(player.scriptID, 5, drawable, 0, 0);

			let BagsTextureNewArray = [];
			for (let i = 0; i < game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)) + 1; i++) {
				BagsTextureNewArray.push(i.toString());
			}
			
			BagsTextureItem.Collection = new ItemsCollection(BagsTextureNewArray).getListItems();
			BagsTextureItem.Index = 0;
			break;
		}
		
		case 1: {
			game.setPedComponentVariation(player.scriptID, 5, drawable, texture, 0);
			break;
		}

		default: {
		}
	}
});

///////////////////////BAGS////////////////////////////////////////


///////////////////////SHOES////////////////////////////////////////
let ShoesDrawable = [];

for (let i = 0; i < game.getNumberOfPedDrawableVariations(player.scriptID, 1) + 1; i++) {
	ShoesDrawable.push(i.toString());
}

let ShoesTextureArray = [];
let ShoesTextureLimit = game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1));
for (let i = 0; i < ShoesTextureLimit + 1; i++) {
	ShoesTextureArray.push(i.toString());
}

const ShoesMenu = new Menu("Shoes Menu", "", new Point(50, 50));
ShoesMenu.Visible = false;
ShoesMenu.GetTitle().Scale = 0.9;
menu.AddSubMenu(ShoesMenu, menushoes);

let ShoesItem = new NativeUI.UIMenuAutoListItem("Shoes", "", -maxListItems, maxListItems, 1, game.getNumberOfPedDrawableVariations(player.scriptID, 1));
let ShoesTextureItem = new NativeUI.UIMenuAutoListItem("Color", "", -maxListItems, maxListItems, 0, game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)));

ShoesMenu.AddItem(ShoesItem);
ShoesMenu.AddItem(ShoesTextureItem);
ShoesMenu.ItemSelect.on((selectedItem, selectedItemIndex) => {
	let drawable = ShoesItem.SelectedValue;
	let texture = ShoesTextureItem.SelectedValue;

	switch (selectedItemIndex) {
		case 0: {
			game.setPedComponentVariation(player.scriptID, 6, drawable, 0, 0);

			let ShoesTextureNewArray = [];
			for (let i = 0; i < game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)) + 1; i++) {
				ShoesTextureNewArray.push(i.toString());
			}
			
			ShoesTextureItem.Collection = new ItemsCollection(ShoesTextureNewArray).getListItems();
			ShoesTextureItem.Index = 0;
			break;
		}
		
		case 1: {
			game.setPedComponentVariation(player.scriptID, 6, drawable, texture, 0);
			break;
		}

		default: {
		}
	}
});

///////////////////////SHOES////////////////////////////////////////


///////////////////////ACCESSOIRES////////////////////////////////////////
let AccDrawable = [];

for (let i = 0; i < game.getNumberOfPedDrawableVariations(player.scriptID, 1) + 1; i++) {
	AccDrawable.push(i.toString());
}

let AccTextureArray = [];
let AccTextureLimit = game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1));
for (let i = 0; i < AccTextureLimit + 1; i++) {
	AccTextureArray.push(i.toString());
}

const AccMenu = new Menu("Accessories Menu", "", new Point(50, 50));
AccMenu.Visible = false;
AccMenu.GetTitle().Scale = 0.9;
menu.AddSubMenu(AccMenu, menuacc);

let AccItem = new NativeUI.UIMenuAutoListItem("Accessories", "", -maxListItems, maxListItems, 1, game.getNumberOfPedDrawableVariations(player.scriptID, 1));
let AccTextureItem = new NativeUI.UIMenuAutoListItem("Color", "", -maxListItems, maxListItems, 0, game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)));

AccMenu.AddItem(AccItem);
AccMenu.AddItem(AccTextureItem);
AccMenu.ItemSelect.on((selectedItem, selectedItemIndex) => {
	let drawable = AccItem.SelectedValue;
	let texture = AccTextureItem.SelectedValue;

	switch (selectedItemIndex) {
		case 0: {
			game.setPedComponentVariation(player.scriptID, 7, drawable, 0, 0);

			let AccTextureNewArray = [];
			for (let i = 0; i < game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)) + 1; i++) {
				AccTextureNewArray.push(i.toString());
			}
			
			AccTextureItem.Collection = new ItemsCollection(AccTextureNewArray).getListItems();
			AccTextureItem.Index = 0;
			break;
		}
		
		case 1: {
			game.setPedComponentVariation(player.scriptID, 7, drawable, texture, 0);
			break;
		}

		default: {
		}
	}
});

///////////////////////ACCESSOIRES////////////////////////////////////////


///////////////////////UNDERSHIRT////////////////////////////////////////
let UnderDrawable = [];

for (let i = 0; i < game.getNumberOfPedDrawableVariations(player.scriptID, 1) + 1; i++) {
	UnderDrawable.push(i.toString());
}

let UnderTextureArray = [];
let UnderTextureLimit = game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1));
for (let i = 0; i < UnderTextureLimit + 1; i++) {
	UnderTextureArray.push(i.toString());
}

const UnderMenu = new Menu("Undershirt Menu", "", new Point(50, 50));
UnderMenu.Visible = false;
UnderMenu.GetTitle().Scale = 0.9;
menu.AddSubMenu(UnderMenu, menuunder);

let UnderItem = new NativeUI.UIMenuAutoListItem("Undershirt", "", -maxListItems, maxListItems, 1, game.getNumberOfPedDrawableVariations(player.scriptID, 1));
let UnderTextureItem = new NativeUI.UIMenuAutoListItem("Color", "", -maxListItems, maxListItems, 0, game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)));

UnderMenu.AddItem(UnderItem);
UnderMenu.AddItem(UnderTextureItem);
UnderMenu.ItemSelect.on((selectedItem, selectedItemIndex) => {
	let drawable = UnderItem.SelectedValue;
	let texture = UnderTextureItem.SelectedValue;

	switch (selectedItemIndex) {
		case 0: {
			game.setPedComponentVariation(player.scriptID, 8, drawable, 0, 0);

			let UnderTextureNewArray = [];
			for (let i = 0; i < game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)) + 1; i++) {
				UnderTextureNewArray.push(i.toString());
			}
			
			UnderTextureItem.Collection = new ItemsCollection(UnderTextureNewArray).getListItems();
			UnderTextureItem.Index = 0;
			break;
		}
		
		case 1: {
			game.setPedComponentVariation(player.scriptID, 8, drawable, texture, 0);
			break;
		}

		default: {
		}
	}
});

///////////////////////UNDERSHIRT////////////////////////////////////////





///////////////////////ARMOR////////////////////////////////////////
let ArmorDrawable = [];

for (let i = 0; i < game.getNumberOfPedDrawableVariations(player.scriptID, 1) + 1; i++) {
	ArmorDrawable.push(i.toString());
}

let ArmorTextureArray = [];
let ArmorTextureLimit = game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1));
for (let i = 0; i < ArmorTextureLimit + 1; i++) {
	ArmorTextureArray.push(i.toString());
}

const ArmorMenu = new Menu("Armor Menu", "", new Point(50, 50));
ArmorMenu.Visible = false;
ArmorMenu.GetTitle().Scale = 0.9;
menu.AddSubMenu(ArmorMenu, menuarmor);

let ArmorItem = new NativeUI.UIMenuAutoListItem("Armor", "", -maxListItems, maxListItems, 1, game.getNumberOfPedDrawableVariations(player.scriptID, 1));
let ArmorTextureItem = new NativeUI.UIMenuAutoListItem("Color", "", -maxListItems, maxListItems, 0, game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)));

ArmorMenu.AddItem(ArmorItem);
ArmorMenu.AddItem(ArmorTextureItem);
ArmorMenu.ItemSelect.on((selectedItem, selectedItemIndex) => {
	let drawable = ArmorItem.SelectedValue;
	let texture = ArmorTextureItem.SelectedValue;

	switch (selectedItemIndex) {
		case 0: {
			game.setPedComponentVariation(player.scriptID, 9, drawable, 0, 0);

			let ArmorTextureNewArray = [];
			for (let i = 0; i < game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)) + 1; i++) {
				ArmorTextureNewArray.push(i.toString());
			}
			
			ArmorTextureItem.Collection = new ItemsCollection(ArmorTextureNewArray).getListItems();
			ArmorTextureItem.Index = 0;
			break;
		}
		
		case 1: {
			game.setPedComponentVariation(player.scriptID, 9, drawable, texture, 0);
			break;
		}

		default: {
		}
	}
});

///////////////////////ARMOR////////////////////////////////////////


///////////////////////DECALS////////////////////////////////////////
let DecalsDrawable = [];

for (let i = 0; i < game.getNumberOfPedDrawableVariations(player.scriptID, 1) + 1; i++) {
	DecalsDrawable.push(i.toString());
}

let DecalsTextureArray = [];
let DecalsTextureLimit = game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1));
for (let i = 0; i < DecalsTextureLimit + 1; i++) {
	DecalsTextureArray.push(i.toString());
}

const DecalsMenu = new Menu("Decals menu", "", new Point(50, 50));
DecalsMenu.Visible = false;
DecalsMenu.GetTitle().Scale = 0.9;
menu.AddSubMenu(DecalsMenu, menudecals);

let DecalsItem = new NativeUI.UIMenuAutoListItem("Decals", "", -maxListItems, maxListItems, 1, game.getNumberOfPedDrawableVariations(player.scriptID, 1));
let DecalsTextureItem = new NativeUI.UIMenuAutoListItem("Color", "", -maxListItems, maxListItems, 0, game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)));

DecalsMenu.AddItem(DecalsItem);
DecalsMenu.AddItem(DecalsTextureItem);
DecalsMenu.ItemSelect.on((selectedItem, selectedItemIndex) => {
	let drawable = DecalsItem.SelectedValue;
	let texture = DecalsTextureItem.SelectedValue;

	switch (selectedItemIndex) {
		case 0: {
			game.setPedComponentVariation(player.scriptID, 10, drawable, 0, 0);

			let DecalsTextureNewArray = [];
			for (let i = 0; i < game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)) + 1; i++) {
				DecalsTextureNewArray.push(i.toString());
			}
			
			DecalsTextureItem.Collection = new ItemsCollection(DecalsTextureNewArray).getListItems();
			DecalsTextureItem.Index = 0;
			break;
		}
		
		case 1: {
			game.setPedComponentVariation(player.scriptID, 10, drawable, texture, 0);
			break;
		}

		default: {
		}
	}
});

///////////////////////DECALS////////////////////////////////////////



///////////////////////TOPS////////////////////////////////////////
let TopsDrawable = [];

for (let i = 0; i < game.getNumberOfPedDrawableVariations(player.scriptID, 1) + 1; i++) {
	TopsDrawable.push(i.toString());
}

let TopsTextureArray = [];
let TopsTextureLimit = game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1));
for (let i = 0; i < TopsTextureLimit + 1; i++) {
	TopsTextureArray.push(i.toString());
}

const TopsMenu = new Menu("Tops Menu", "", new Point(50, 50));
TopsMenu.Visible = false;
TopsMenu.GetTitle().Scale = 0.9;
menu.AddSubMenu(TopsMenu, menutops);

let TopsItem = new NativeUI.UIMenuAutoListItem("Tops", "", -maxListItems, maxListItems, 1, game.getNumberOfPedDrawableVariations(player.scriptID, 1));
let TopsTextureItem = new NativeUI.UIMenuAutoListItem("Color", "", -maxListItems, maxListItems, 0, game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)));

TopsMenu.AddItem(TopsItem);
TopsMenu.AddItem(TopsTextureItem);
TopsMenu.ItemSelect.on((selectedItem, selectedItemIndex) => {
	let drawable = TopsItem.SelectedValue;
	let texture = TopsTextureItem.SelectedValue;

	switch (selectedItemIndex) {
		case 0: {
			game.setPedComponentVariation(player.scriptID, 11, drawable, 0, 0);

			let TopsTextureNewArray = [];
			for (let i = 0; i < game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)) + 1; i++) {
				TopsTextureNewArray.push(i.toString());
			}
			
			TopsTextureItem.Collection = new ItemsCollection(TopsTextureNewArray).getListItems();
			TopsTextureItem.Index = 0;
			break;
		}
		
		case 1: {
			game.setPedComponentVariation(player.scriptID, 11, drawable, texture, 0);
			break;
		}

		default: {
		}
	}
});

///////////////////////TOPS////////////////////////////////////////



///////////////////////PROPS HAT////////////////////////////////////////
let HatDrawable = [];

for (let i = 0; i < game.getNumberOfPedDrawableVariations(player.scriptID, 1) + 1; i++) {
	HatDrawable.push(i.toString());
}

let HatTextureArray = [];
let HatTextureLimit = game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1));
for (let i = 0; i < HatTextureLimit + 1; i++) {
	HatTextureArray.push(i.toString());
}

const HatMenu = new Menu("Hats Menu", "", new Point(50, 50));
HatMenu.Visible = false;
HatMenu.GetTitle().Scale = 0.9;
menu.AddSubMenu(HatMenu, menuhat);

let HatItem = new NativeUI.UIMenuAutoListItem("Hats", "", -maxListItems, maxListItems, 1, game.getNumberOfPedDrawableVariations(player.scriptID, 1));
let HatTextureItem = new NativeUI.UIMenuAutoListItem("Color", "", -maxListItems, maxListItems, 0, game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)));

HatMenu.AddItem(HatItem);
HatMenu.AddItem(HatTextureItem);
HatMenu.ItemSelect.on((selectedItem, selectedItemIndex) => {
	let drawable = HatItem.SelectedValue;
	let texture = HatTextureItem.SelectedValue;

	switch (selectedItemIndex) {
		case 0: {
			game.setPedPropIndex(player.scriptID, 0, drawable, 0, 0);

			let HatTextureNewArray = [];
			for (let i = 0; i < game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)) + 1; i++) {
				HatTextureNewArray.push(i.toString());
			}
			
			HatTextureItem.Collection = new ItemsCollection(HatTextureNewArray).getListItems();
			HatTextureItem.Index = 0;
			break;
		}
		
		case 1: {
			game.setPedPropIndex(player.scriptID, 0, drawable, texture, 0);
			break;
		}

		default: {
		}
	}
});

///////////////////////PROPS HAT////////////////////////////////////////


///////////////////////PROPS GLASSES////////////////////////////////////////
let GlassesDrawable = [];

for (let i = 0; i < game.getNumberOfPedDrawableVariations(player.scriptID, 1) + 1; i++) {
	GlassesDrawable.push(i.toString());
}

let GlassesTextureArray = [];
let GlassesTextureLimit = game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1));
for (let i = 0; i < GlassesTextureLimit + 1; i++) {
	GlassesTextureArray.push(i.toString());
}

const GlassesMenu = new Menu("Glasses Menu", "", new Point(50, 50));
GlassesMenu.Visible = false;
GlassesMenu.GetTitle().Scale = 0.9;
menu.AddSubMenu(GlassesMenu, menuglasses);

let GlassesItem = new NativeUI.UIMenuAutoListItem("Glasses", "", -maxListItems, maxListItems, 1, game.getNumberOfPedDrawableVariations(player.scriptID, 1));
let GlassesTextureItem = new NativeUI.UIMenuAutoListItem("Color", "", -maxListItems, maxListItems, 0, game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)));

GlassesMenu.AddItem(GlassesItem);
GlassesMenu.AddItem(GlassesTextureItem);
GlassesMenu.ItemSelect.on((selectedItem, selectedItemIndex) => {
	let drawable = GlassesItem.SelectedValue;
	let texture = GlassesTextureItem.SelectedValue;

	switch (selectedItemIndex) {
		case 0: {
			game.setPedPropIndex(player.scriptID, 1, drawable, 0, 0);

			let GlassesTextureNewArray = [];
			for (let i = 0; i < game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)) + 1; i++) {
				GlassesTextureNewArray.push(i.toString());
			}
			
			GlassesTextureItem.Collection = new ItemsCollection(GlassesTextureNewArray).getListItems();
			GlassesTextureItem.Index = 0;
			break;
		}
		
		case 1: {
			game.setPedPropIndex(player.scriptID, 1, drawable, texture, 0);
			break;
		}

		default: {
		}
	}
});

///////////////////////PROPS GLASSES////////////////////////////////////////



///////////////////////PROPS Ears////////////////////////////////////////
let EarsDrawable = [];

for (let i = 0; i < game.getNumberOfPedDrawableVariations(player.scriptID, 1) + 1; i++) {
	EarsDrawable.push(i.toString());
}

let EarsTextureArray = [];
let EarsTextureLimit = game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1));
for (let i = 0; i < EarsTextureLimit + 1; i++) {
	EarsTextureArray.push(i.toString());
}

const EarsMenu = new Menu("Ears Menu", "", new Point(50, 50));
EarsMenu.Visible = false;
EarsMenu.GetTitle().Scale = 0.9;
menu.AddSubMenu(EarsMenu, menuears);

let EarsItem = new NativeUI.UIMenuAutoListItem("Ears", "", -maxListItems, maxListItems, 1, game.getNumberOfPedDrawableVariations(player.scriptID, 1));
let EarsTextureItem = new NativeUI.UIMenuAutoListItem("Color", "", -maxListItems, maxListItems, 0, game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)));

EarsMenu.AddItem(EarsItem);
EarsMenu.AddItem(EarsTextureItem);
EarsMenu.ItemSelect.on((selectedItem, selectedItemIndex) => {
	let drawable = EarsItem.SelectedValue;
	let texture = EarsTextureItem.SelectedValue;

	switch (selectedItemIndex) {
		case 0: {
			game.setPedPropIndex(player.scriptID, 2, drawable, 0, 0);

			let EarsTextureNewArray = [];
			for (let i = 0; i < game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)) + 1; i++) {
				EarsTextureNewArray.push(i.toString());
			}
			
			EarsTextureItem.Collection = new ItemsCollection(EarsTextureNewArray).getListItems();
			EarsTextureItem.Index = 0;
			break;
		}
		
		case 1: {
			game.setPedPropIndex(player.scriptID, 2, drawable, texture, 0);
			break;
		}

		default: {
		}
	}
});

///////////////////////PROPS Ears////////////////////////////////////////




///////////////////////PROPS Wacthes////////////////////////////////////////
let WatchesDrawable = [];

for (let i = 0; i < game.getNumberOfPedDrawableVariations(player.scriptID, 1) + 1; i++) {
	WatchesDrawable.push(i.toString());
}

let WatchesTextureArray = [];
let WatchesTextureLimit = game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1));
for (let i = 0; i < WatchesTextureLimit + 1; i++) {
	WatchesTextureArray.push(i.toString());
}

const WatchesMenu = new Menu("Watches Menu", "", new Point(50, 50));
WatchesMenu.Visible = false;
WatchesMenu.GetTitle().Scale = 0.9;
menu.AddSubMenu(WatchesMenu, menuwatches);

let WatchesItem = new NativeUI.UIMenuAutoListItem("Watches", "", -maxListItems, maxListItems, 1, game.getNumberOfPedDrawableVariations(player.scriptID, 1));
let WatchesTextureItem = new NativeUI.UIMenuAutoListItem("Color", "", -maxListItems, maxListItems, 0, game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)));

WatchesMenu.AddItem(WatchesItem);
WatchesMenu.AddItem(WatchesTextureItem);
WatchesMenu.ItemSelect.on((selectedItem, selectedItemIndex) => {
	let drawable = WatchesItem.SelectedValue;
	let texture = WatchesTextureItem.SelectedValue;

	switch (selectedItemIndex) {
		case 0: {
			game.setPedPropIndex(player.scriptID, 6, drawable, 0, 0);

			let WatchesTextureNewArray = [];
			for (let i = 0; i < game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)) + 1; i++) {
				WatchesTextureNewArray.push(i.toString());
			}
			
			WatchesTextureItem.Collection = new ItemsCollection(WatchesTextureNewArray).getListItems();
			WatchesTextureItem.Index = 0;
			break;
		}
		
		case 1: {
			game.setPedPropIndex(player.scriptID, 6, drawable, texture, 0);
			break;
		}

		default: {
		}
	}
});

///////////////////////PROPS Watches////////////////////////////////////////



///////////////////////PROPS Bracelets////////////////////////////////////////
let BraceletsDrawable = [];

for (let i = 0; i < game.getNumberOfPedDrawableVariations(player.scriptID, 1) + 1; i++) {
	BraceletsDrawable.push(i.toString());
}

let BraceletsTextureArray = [];
let BraceletsTextureLimit = game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1));
for (let i = 0; i < BraceletsTextureLimit + 1; i++) {
	BraceletsTextureArray.push(i.toString());
}

const BraceletsMenu = new Menu("Bracelets Menu", "", new Point(50, 50));
BraceletsMenu.Visible = false;
BraceletsMenu.GetTitle().Scale = 0.9;
menu.AddSubMenu(BraceletsMenu, menubracelets);

let BraceletsItem = new NativeUI.UIMenuAutoListItem("Bracelets", "", -maxListItems, maxListItems, 1, game.getNumberOfPedDrawableVariations(player.scriptID, 1));
let BraceletsTextureItem = new NativeUI.UIMenuAutoListItem("Color", "", -maxListItems, maxListItems, 0, game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)));

BraceletsMenu.AddItem(BraceletsItem);
BraceletsMenu.AddItem(BraceletsTextureItem);
BraceletsMenu.ItemSelect.on((selectedItem, selectedItemIndex) => {
	let drawable = BraceletsItem.SelectedValue;
	let texture = BraceletsTextureItem.SelectedValue;

	switch (selectedItemIndex) {
		case 0: {
			game.setPedPropIndex(player.scriptID, 7, drawable, 0, 0);

			let BraceletsTextureNewArray = [];
			for (let i = 0; i < game.getNumberOfPedTextureVariations(player.scriptID, 1, game.getPedDrawableVariation(player.scriptID, 1)) + 1; i++) {
				BraceletsTextureNewArray.push(i.toString());
			}
			
			BraceletsTextureItem.Collection = new ItemsCollection(BraceletsTextureNewArray).getListItems();
			BraceletsTextureItem.Index = 0;
			break;
		}
		
		case 1: {
			game.setPedPropIndex(player.scriptID, 7, drawable, texture, 0);
			break;
		}

		default: {
		}
	}
});

///////////////////////PROPS Bracelets////////////////////////////////////////

alt.on('keyup', (key) => {
    if (key === 0x71) { //F2 KEY
        if (menu.Visible)
            menu.Close();
        else
            menu.Open();
    }
});