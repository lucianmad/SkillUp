import { Routes } from '@angular/router';
import { HomeComponent } from './features/home/home.component';
import { QuizTakerComponent } from './features/quiz-taker/quiz-taker.component';
import { ResultsComponent } from './features/results/results.component';
import { QuizListComponent } from './features/quiz-list/quiz-list.component';

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
    {
        path: 'quiz/:id/results',
        component: ResultsComponent,
        title: 'Quiz Results'
    },
    {
        path: 'quizzes',
        component: QuizListComponent,
        title: 'All Quizzes'
    }
];
