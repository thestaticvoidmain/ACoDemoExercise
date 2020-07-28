import { Component, OnInit } from '@angular/core';
import { IPackage } from './models/package';
import { BasketService } from './basket/basket.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit{

  title = 'DemoClient';

  constructor(private basketService: BasketService) {}

  ngOnInit(): void {
    const basket = JSON.parse(localStorage.getItem('basket'));
  }
}
