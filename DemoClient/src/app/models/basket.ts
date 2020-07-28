export interface IBasket {
  id: string;
  items: IBasketItem[];
}

export interface IBasketItem {
  packageId: number;
  packageName: string;
  price: number;
  quantity: number;
  imgURL: string;
}

export class Basket implements IBasket {
  id = 'basketDemoId';
  items: IBasketItem[] = [];
}

export class DiscountApiResult {
  id: 'discountApiResult';
  items: IBasketDiscount[];
}

export interface IBasketDiscount {
  packageId: number;
  addedQuantity: number;
  discountedPrice: number;
}

export interface IBasketDiscountSummary {
  totalBeforeDiscount: number;
  subtotalDiscounts: number;
  subtotalQty: number;
  totalPayable: number;
}
