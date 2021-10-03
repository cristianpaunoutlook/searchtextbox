import { request } from './agent'

export const Products = {
    list: (serchedText: string, cancelTokenSource) => request.get(`/products/${serchedText}`, cancelTokenSource),
    incrementWeight: (productId: number) => request.put('products', {id: productId}),
}