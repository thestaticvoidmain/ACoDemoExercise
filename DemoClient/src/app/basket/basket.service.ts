import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { IPackage } from '../models/package';
import { Basket } from '../models/basket';
import { IBasketItem } from '../models/basket';
import { IBasketDiscount } from '../models/basket';
import { IBasketDiscountSummary } from '../models/basket';
import { DiscountApiResult } from '../models/basket';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;
  basketValue = new BehaviorSubject(this.theBasket);

  private discountApiResult = new BehaviorSubject<IBasketDiscount[]>(null);
  private discountSummaryValue = new BehaviorSubject<IBasketDiscountSummary>(null);
  discountSummary$ = this.discountSummaryValue.asObservable();

  constructor(private http: HttpClient) { }

  set theBasket(value) {
    this.basketValue.next(value);
    localStorage.setItem('basket', JSON.stringify(value));
    this.calculateBasketDiscount(value.items);
  }

  get theBasket() {
    return JSON.parse(localStorage.getItem('basket'));
  }

  getCurrentBasket() {
    console.log('get current basket');
    if (localStorage.getItem('basket') === null){
      return null;
    }
    else {
      return JSON.parse(localStorage.getItem('basket'));
    }
  }

  addItemToBasket(item: IPackage, quantity = 1){
    const itemToAdd: IBasketItem = this.mapPackageToBasketItem(item, quantity);
    let basket = this.getCurrentBasket();
    if (basket === null) {
      basket = this.createBasket();
    }
    basket.items = this.addOrUpdateItem(basket.items, itemToAdd, quantity);
    this.theBasket = basket;
  }

  private mapPackageToBasketItem(item: IPackage, quantity: number): IBasketItem {
    return {
      packageId: item.id,
      packageName: item.name,
      price: item.price,
      imgURL: item.imgURL,
      quantity,
    };
  }

  private createBasket() {
    console.log('creating new basket');
    const basket = new Basket();
    localStorage.setItem('basket', JSON.stringify(basket));
  }

  private addOrUpdateItem(items: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    const index = items.findIndex(i => i.packageId === itemToAdd.packageId);
    if (index === -1) {
      itemToAdd.quantity = quantity;
      items.push(itemToAdd);
    } else {
      items[index].quantity += quantity;
    }
    return items;
  }

  private calculateBasketDiscount(items: IBasketItem[]) {
    console.log('call post method');
    const headers = { 'content-type': 'application/json'};
    const body = JSON.stringify(items);

    return this.http.post(this.baseUrl + 'promos/applyPromo', body, { ['headers']: headers}).subscribe((response: IBasketDiscount[]) => {
      this.discountApiResult.next(response);
      this.showDiscountSummary();
    }, error => {
      console.log(error);
    });
  }

  private showDiscountSummary() {
    console.log('show discount summary');
    const basket = this.getCurrentBasket();
    const s = new DiscountApiResult();
    s.items = this.discountApiResult.getValue();
    const totalBeforeDiscount = basket.items.reduce((a, b) => (b.price * b.quantity) + a, 0);
    const subtotalDiscounts = s.items.reduce((a, b) => (b.discountedPrice) + a, 0);
    const subtotalQty = s.items.reduce((a, b) => (b.addedQuantity) + a, 0);
    const totalPayable = totalBeforeDiscount - subtotalDiscounts;
    this.discountSummaryValue.next({totalBeforeDiscount, subtotalDiscounts, subtotalQty, totalPayable});
  }

  incrementItemQuantity(item: IBasketItem) {
    const basket = this.getCurrentBasket();
    const foundItemIndex = basket.items.findIndex(x => x.packageId === item.packageId);
    basket.items[foundItemIndex].quantity++;
    this.theBasket = basket;
  }

  decrementItemQuantity(item: IBasketItem) {
    const basket = this.getCurrentBasket();
    const foundItemIndex = basket.items.findIndex(x => x.packageId === item.packageId);
    if (basket.items[foundItemIndex].quantity > 1) {
      basket.items[foundItemIndex].quantity--;
      this.theBasket = basket;
    } else {
      this.removeItemFromBasket(item);
    }
  }

  removeItemFromBasket(item: IBasketItem) {
    const basket = this.getCurrentBasket();
    if (basket.items.some(x => x.packageId === item.packageId)) {
      basket.items = basket.items.filter(i => i.packageId !== item.packageId);
      if (basket.items.length > 0) {
        this.theBasket = basket;
      } else {
        this.deleteBasket(basket);
      }
    }
  }

  deleteBasket(basket: Basket)
  {
    this.theBasket = null;
    localStorage.clear();
  }
}
