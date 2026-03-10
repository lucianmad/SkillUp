export interface QuizListResponse {
    id: string;
    title: string;
    questionCount: number;
}


export interface AnswerListResponse {
    id: string;
    text: string;
}

export interface QuestionListResponse {
    id: string;
    text: string;
    type: string;
    answers: AnswerListResponse[];
}


export interface SubmitAnswerItem {
    questionId: string;
    selectedAnswersId: string[];
}

export interface SubmitAnswersRequest {
    answers: SubmitAnswerItem[];
}

export interface SubmitAnswersResponse {
    score: number;
    totalQuestions: number;
}

