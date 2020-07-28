import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopComponent } from './shop.component';
import { ItemComponent } from './item/item.component';



@NgModule({
  declarations: [ShopComponent, ItemComponent],
  imports: [
    CommonModule
  ],
  exports: [ShopComponent]
})
export class ShopModule { }
