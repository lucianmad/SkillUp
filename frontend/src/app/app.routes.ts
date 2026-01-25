import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { QuizTakerComponent } from './features/quiz-taker/quiz-taker.component';

export const routes: Routes = [
    {
        path: '',
        component: HomeComponent,
        title: 'SkillUp - Home'
    },
    {
        path: 'quiz/:id',
        component: QuizTakerComponent,
        title: 'Taking Quiz'
    },
];
