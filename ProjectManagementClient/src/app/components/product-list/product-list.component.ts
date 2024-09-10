import { Component, OnInit } from '@angular/core';
import { Product } from '../../models/product.model';
import { ProductService } from '../../services/product.service';
import { ToastrService } from 'ngx-toastr';
import { NgFor } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ProductItemComponent } from "../product-item/product-item.component";
import { HttpResponse } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-product-list',
  standalone: true,
  imports: [NgFor, RouterLink, ProductItemComponent, FormsModule],
  templateUrl: './product-list.component.html',
  styleUrl: './product-list.component.css'
})
export class ProductListComponent implements OnInit {
  products: Product[]=[];
  searchName: string = '';
  sortBy: string = 'name';
  sortAsc: boolean = true;

  constructor(private productService: ProductService, private toastr: ToastrService){}

  ngOnInit(): void {
    this.getProducts();
  }

  getProducts(): void {
    this.productService.getProducts().subscribe(products => this.products = products);
  }

  deleteProduct(id: number): void {
    this.productService.deleteProduct(id).subscribe({
      next: (response: HttpResponse<any>) => {
        if(response.status === 204) {
          this.toastr.success('Product deleted successfully');
          this.getProducts();
        } else {
          this.toastr.error('Product deletion failed');
        }
      },
      error: (error) => {
        this.toastr.error('Product deletion failed');
      }
    });
  }
  
  deleteAllProducts(): void {
    this.productService.deleteProducts().subscribe(result => {
      if (result) {
        this.toastr.success('All products deleted successfully');
        this.getProducts();
      } else {
        this.toastr.error('All products deletion failed');
      }
    });
  }

  searchProducts(): void{
    if(this.searchName){
      this.productService.searchProduct(this.searchName).subscribe(products => this.products = products);
    } else {
      this.getProducts();
    }
  }

  sortProducts(): void {
    this.productService.getSortedProducts(this.sortBy, this.sortAsc).subscribe(products => this.products = products);
  }

  onSortChange(event: any): void {
    this.sortBy = event.target.value;
    this.sortProducts();
  }

  onSortOrderChange(event: any): void {
    this.sortAsc = event.target.checked;
    this.sortProducts();
  }  

}
