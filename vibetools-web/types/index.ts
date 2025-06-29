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

export interface ToolCreateErrors {
    name?: string;
    description?: string;
    server?: string
}

export interface ReviewCreateErrors {
    comment?: string;
    rating?: string;
    server?: string
}