import { Component, OnInit } from '@angular/core';
import { ShopService } from './shop.service';
import { IPackage } from '../models/package';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {
  packages: IPackage[];

  constructor(private shopService: ShopService) { }

  ngOnInit() {
    this.shopService.getPackages().subscribe(response => {
        this.packages = response;
      }, error => {
        console.log(error);
      });
    }
  }
