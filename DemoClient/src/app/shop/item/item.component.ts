import { Component, OnInit, Input } from '@angular/core';
import { IPackage } from 'src/app/models/package';
import { BasketService } from '../../basket/basket.service';

@Component({
  selector: 'app-item',
  templateUrl: './item.component.html',
  styleUrls: ['./item.component.scss']
})
export class ItemComponent implements OnInit {
  @Input() package: IPackage;

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
  }

  addItemToBasket() {
    this.basketService.addItemToBasket(this.package);
  }

}
