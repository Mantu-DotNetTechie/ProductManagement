import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ProductService } from '../../services/product.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Product } from '../../models/product.model';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.css'
})
export class ProductFormComponent implements OnInit {

  product: Product = {
    name: '',
    description: '',
    category: '',
    price: 0,
    id: 0
  };

  isEditMode = false;
  productId: number = 0;

  constructor(
    private productService: ProductService,
    private route: ActivatedRoute,
    private router: Router,
    private toastr: ToastrService
  ) { }


  ngOnInit(): void {
    debugger;
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      this.productId = id ? parseInt(id, 10) : 0;
      this.isEditMode = this.productId ? true : false;
      if (this.isEditMode) {
          this.loadProduct();
      }

  });
}

  // initForm(): void {
  //   debugger;
  //   this.productForm = this.fb.group({
  //     name: ['', Validators.required],
  //     description: ['', Validators.required],
  //     category: ['', Validators.required],
  //     price: [0, [Validators.required, Validators.min(0.01)]]
  //   });
  // }

    loadProduct(): void {
      if(this.productId !== 0) { 
        debugger;
        this.productService.getProductById(this.productId).subscribe(product => {
          this.product = product;
        });
      }
    }
    

  onSubmit(form: NgForm): void {
    if (!form.valid) {
      this.toastr.error('Please enter valid data');
      return;
    }

    if (this.isEditMode && this.productId !== 0) {
      this.productService.updateProduct(this.productId, this.product).subscribe({
        next: (response: HttpResponse<any>) => {
          if (response.status === 204) {
            this.toastr.success('Product updated successfully');
            this.router.navigate(['/products']);
          } else {
            this.toastr.error('Product update failed');
          }
        },  
        error: () => {
          this.toastr.error('Product update failed');
        }
      });
    } else {
      this.productService.addProduct(this.product).subscribe({
        next: (response: HttpResponse<any>) => {
          debugger
          if (response.status === 201) {
            this.toastr.success('Product added successfully');
            this.router.navigate(['/products']);
          } else {
            this.toastr.error('Product addition failed');
          }
        },
        error: () => {
          this.toastr.error('Product addition failed');
        }
      });
    }
  }

  onCancel(): void {
    this.router.navigate(['/products']);
  }

}
