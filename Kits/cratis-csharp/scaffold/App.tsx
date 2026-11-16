import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { DialogComponents } from '@cratis/arc.react/dialogs';
import { BusyIndicatorDialog, ConfirmationDialog } from '@cratis/components/Dialogs';

// Each built feature adds its route here.
function App() {
    return (
        <DialogComponents confirmation={ConfirmationDialog} busyIndicator={BusyIndicatorDialog}>
            <BrowserRouter>
                <Routes>
                    <Route path='/' element={<main />} />
                </Routes>
            </BrowserRouter>
        </DialogComponents>
    );
}

export default App;
