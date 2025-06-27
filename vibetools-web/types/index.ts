export interface Review {
    id: number;
    rating: number;
    comment: string;
    createdAt: string;
}

export interface Tool {
    id: number;
    name: string;
    description: string;
    createdAt: string;
    avgRating: number;
    reviewCount: number;
    communityFavourite: boolean;
    hidden: boolean;
    reviews?: Review[];
}