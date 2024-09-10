import { HttpClient, HttpParams, HttpResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Product } from "../models/product.model";
import { Observable } from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ProductService {
    private apiUrl = 'https://localhost:7273/api/products';
  constructor(private http: HttpClient) { }  

    // Get all products
    getProducts(): Observable<Product[]> {
        return this.http.get<Product[]>(this.apiUrl);
    }

    // Get product by ID
    getProductById(id: number): Observable<Product> {
        return this.http.get<Product>(`${this.apiUrl}/${id}`);
    }

    // add product
    addProduct(product: Product): Observable<HttpResponse<any>> {
        return this.http.post<Product>(this.apiUrl, product, { observe: 'response' });
    }

    // update product
    updateProduct(id: number, product: Product): Observable<HttpResponse<any>> {
        return this.http.put<boolean>(`${this.apiUrl}/${id}`, product, { observe: 'response' });
    }

    // delete product
    deleteProduct(id?: number): Observable<HttpResponse<any>> {
        return this.http.delete<boolean>(`${this.apiUrl}/${id}`, { observe: 'response' });
    }

    // delete all products
    deleteProducts(): Observable<HttpResponse<any>> {
        return this.http.delete<boolean>(this.apiUrl, { observe: 'response' });
    }

    // search product
    searchProduct(name: string): Observable<Product[]> {
        let params = new HttpParams().set('name', name);
        return this.http.get<Product[]>(`${this.apiUrl}/search`,{ params });
    }

    // get product by category
    getProductByCategory(category: string): Observable<Product[]> {
        return this.http.get<Product[]>(`${this.apiUrl}/category/${category}`);
    }

    // get total count of products
    getTotalCount(): Observable<number> {
        return this.http.get<number>(`${this.apiUrl}/total-count`);
    }

    // get sorted products
    getSortedProducts(sortBy: string, isAscending: boolean): Observable<Product[]> {
        let params = new HttpParams().set('sortBy', sortBy).set('isAscending', isAscending.toString());
        return this.http.get<Product[]>(`${this.apiUrl}/sort`, { params });
    }
  
}